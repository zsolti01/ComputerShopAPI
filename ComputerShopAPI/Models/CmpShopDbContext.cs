using Microsoft.EntityFrameworkCore;

namespace ComputerShopAPI.Models
{
    public class CmpShopDbContext : DbContext
    {
        public CmpShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected CmpShopDbContext()
        {
        }

        public DbSet<Osystem> Osystems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=computer;user=root;password=");
        }
    }
}
