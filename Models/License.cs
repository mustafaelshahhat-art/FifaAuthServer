namespace FifaAuthServer.Models
{
    public class License
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? HWID { get; set; } = null;
        public bool IsUsed => !string.IsNullOrEmpty(HWID);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ActivatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int RenewalCount { get; set; } = 0;
        public int MaxRenewals { get; set; } = 3;
        public bool AutoRenewAllowed { get; set; } = false;
    }
}
