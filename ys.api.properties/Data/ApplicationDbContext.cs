using Microsoft.EntityFrameworkCore;
using ys.api.properties.Models;

namespace ys.api.properties.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyModelEntity>().ToTable("ys-properties"); // Ensures correct table Name

            // Configure geometry column types
            modelBuilder.Entity<PropertyModelEntity>()
                .Property(p => p.location)
                .HasColumnType("geometry(Point, 4326)");

            modelBuilder.Entity<PropertyModelEntity>()
                .Property(p => p.boundary)
                .HasColumnType("geometry(Polygon, 4326)");

            // Configure self-referencing foreign key for parent_id
            modelBuilder.Entity<PropertyModelEntity>()
                .HasOne<PropertyModelEntity>()
                .WithMany()
                .HasForeignKey(p => p.parent_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Address data lookup tables
            modelBuilder.Entity<CountryDataEntity>().ToTable("ys-addresses-country-data");
            modelBuilder.Entity<RegionEntity>().ToTable("ys-addresses-regions");
        }

        public DbSet<PropertyModelEntity> Properties { get; set; }
        public DbSet<CountryDataEntity> CountryData { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
    }
}
