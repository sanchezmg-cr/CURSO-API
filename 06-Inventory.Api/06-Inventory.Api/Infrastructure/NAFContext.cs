using _06_Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace _06_Inventory.Api.Infrastructure
{
    public class NAFContext : DbContext
    {
        public NAFContext(DbContextOptions<NAFContext> options) : base(options) { }

        public DbSet<CATEGORIA> CATEGORIA { get; set; }
        public DbSet<ARTICULOS> ARTICULOS { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
      
            modelBuilder.Entity<CATEGORIA>().HasKey(ba => new { ba.CODIGO });
            modelBuilder.Entity<ARTICULOS>().HasKey(ba => new { ba.CODIGO });
        }
    }
}
