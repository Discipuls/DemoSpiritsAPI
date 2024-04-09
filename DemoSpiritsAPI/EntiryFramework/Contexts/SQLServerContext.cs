using SpiritsClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoSpiritsAPI.EntiryFramework.Contexts
{
    public class SQLServerContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<Spirit> Spirits { get; set; }
        public DbSet<Habitat> Habitats { get; set; }
        public DbSet<MarkerPoint> MarkerPoints { get; set; }
        public DbSet<BorderPoint> BorderPoints { get; set; }

        public SQLServerContext(IConfiguration Configuration) {
            _configuration = Configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:SqlServer"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Spirit>().HasOne(m => m.MarkerLocation).WithOne(e => e.spirit)
    .HasForeignKey<MarkerPoint>("SpiritId");
        }
    }
}
