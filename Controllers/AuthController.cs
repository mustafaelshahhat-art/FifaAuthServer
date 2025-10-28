using Microsoft.AspNetCore.Mvc;
using FifaAuthServer.Data;
using FifaAuthServer.Models;

namespace FifaAuthServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("activate")]
        public IActionResult Activate([FromBody] ActivationRequest req)
        {
            var lic = _context.Licenses.FirstOrDefault(l => l.Code == req.Code);
            if (lic == null)
                return BadRequest(new { message = "Invalid code" });

            if (lic.IsUsed)
            {
                if (lic.HWID == req.HWID)
                    return Ok(new { message = "Already activated", success = true });
                return BadRequest(new { message = "Code already used" });
            }

            lic.HWID = req.HWID;
            lic.ActivatedAt = DateTime.UtcNow;
            lic.ExpiresAt = DateTime.UtcNow.AddMonths(6);
            _context.SaveChanges();

            return Ok(new { message = "Activation successful", success = true });
        }

        [HttpGet("check")]
        public IActionResult Check([FromQuery] string code, [FromQuery] string hwid)
        {
            var lic = _context.Licenses.FirstOrDefault(l => l.Code == code);
            if (lic == null) return BadRequest(new { message = "Invalid code" });
            if (!lic.IsUsed) return BadRequest(new { message = "Code not activated" });
            if (lic.HWID != hwid) return BadRequest(new { message = "Wrong device" });
            if (lic.ExpiresAt < DateTime.UtcNow) return BadRequest(new { message = "License expired" });

            return Ok(new { message = "Valid license", success = true });
        }

        public class ActivationRequest
        {
            public string Code { get; set; } = "";
            public string HWID { get; set; } = "";
        }
    }
}
