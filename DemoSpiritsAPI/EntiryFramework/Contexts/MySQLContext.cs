using DemoSpiritsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoSpiritsAPI.EntiryFramework.Contexts
{
    public class MySQLContext : DbContext
    {
        public DbSet<Spirit> Spirits { get; set; }
        public DbSet<Habitat> Habitats { get; set; }
        public DbSet<MarkerPoint> MarkerPoints { get; set; }
        public DbSet<BorderPoint> BorderPoints { get; set; }

        public MySQLContext() {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=!@#Palych;database=spirits",
                new MySqlServerVersion(new Version(8, 0, 11)));
            //TODO move configuration to json
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          //  modelBuilder.Entity<MarkerPoint>().HasOne(m => m.Habitat).WithOne(e => e.MarkerLocation);
            //modelBuilder.Entity<Habitat>();
            modelBuilder.Entity<Spirit>().HasOne(m => m.MarkerLocation).WithOne(e => e.spirit)
    .HasForeignKey<MarkerPoint>("SpiritId");
            /*
                        Spirit spirit1 = new() { Id = 1 };
                        Spirit spirit2 = new() { Id = 2 };

                        Habitat habitat1 = new() { Id = 1 };
                        Habitat habitat2 = new() { Id = 2 };

                        spirit1.Habitats.Add(habitat1);
                        spirit2.Habitats.Add(habitat2);

                        habitat1.Spirits.Add(spirit1);
                        habitat2.Spirits.Add(spirit2);

                        modelBuilder.Entity<Spirit>().HasData(spirit1, spirit2);
                        modelBuilder.Entity<Habitat>().HasData(habitat1, habitat2);*/



        }
    }
}
