namespace FifaAuthServer.Models
{
    public class RenewRequest
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string HWID { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public bool Processed { get; set; } = false;
        public bool Approved { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
    }
}
