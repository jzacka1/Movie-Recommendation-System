using Microsoft.EntityFrameworkCore;

namespace SmartReturns.Models
{
    public class ReturnsDbContext : DbContext
    {
        public ReturnsDbContext(DbContextOptions<ReturnsDbContext> options) : base(options) { }

        public DbSet<ReturnRequest> Returns { get; set; }
    }
}
