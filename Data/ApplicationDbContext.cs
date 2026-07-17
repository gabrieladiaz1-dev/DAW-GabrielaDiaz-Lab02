using Microsoft.EntityFrameworkCore;
using GestionCalidad.Models;

namespace GestionCalidad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inspection> Inspections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inspection>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Producto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Inspector).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Observaciones).IsRequired().HasMaxLength(500);
            });
        }
    }
}
