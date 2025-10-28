using Microsoft.EntityFrameworkCore;
using FifaAuthServer.Models;

namespace FifaAuthServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<License> Licenses { get; set; }
        public DbSet<RenewRequest> RenewRequests { get; set; }
    }
}
