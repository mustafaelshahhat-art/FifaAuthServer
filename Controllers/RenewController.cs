using Microsoft.AspNetCore.Mvc;
using FifaAuthServer.Data;
using FifaAuthServer.Models;

namespace FifaAuthServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RenewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RenewController(AppDbContext context)
        {
            _context = context;
        }

        // 📨 المستخدم يطلب تجديد الكود
        [HttpPost("request")]
        public IActionResult RequestRenew([FromBody] RenewRequest model)
        {
            if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.HWID))
                return BadRequest(new { message = "Code and HWID required" });

            var license = _context.Licenses.FirstOrDefault(l => l.Code == model.Code);
            if (license == null)
                return BadRequest(new { message = "Invalid code" });

            if (license.HWID != model.HWID)
                return Unauthorized(new { message = "HWID mismatch" });

            if (license.ExpiresAt.HasValue && license.ExpiresAt.Value > DateTime.UtcNow)
                return BadRequest(new { message = "License not expired yet" });

            var existing = _context.RenewRequests
                .FirstOrDefault(r => r.Code == model.Code && !r.Processed);

            if (existing != null)
                return BadRequest(new { message = "Pending renew request already exists" });

            var req = new RenewRequest
            {
                Code = model.Code,
                HWID = model.HWID,
                RequestedAt = DateTime.UtcNow,
                Notes = model.Notes,
                Processed = false,
                Approved = false
            };

            _context.RenewRequests.Add(req);
            _context.SaveChanges();

            return Ok(new { message = "Renew request submitted successfully", requestId = req.Id });
        }

        // 👀 عرض طلبات التجديد (استخدامها في الاختبار فقط)
        [HttpGet("list")]
        public IActionResult List()
        {
            return Ok(_context.RenewRequests.OrderByDescending(r => r.RequestedAt).ToList());
        }
    }
}
