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

            // Map meta_data columns to jsonb so Npgsql sends the correct type
            modelBuilder.Entity<PropertyModelEntity>()
                .Property(p => p.meta_data)
                .HasColumnType("jsonb");

            // Address data lookup tables
            modelBuilder.Entity<CountryDataEntity>().ToTable("ys-addresses-country-data");
            modelBuilder.Entity<CountryDataEntity>()
                .Property(p => p.meta_data)
                .HasColumnType("jsonb");

            modelBuilder.Entity<RegionEntity>().ToTable("ys-addresses-regions");
            modelBuilder.Entity<RegionEntity>()
                .Property(p => p.meta_data)
                .HasColumnType("jsonb");
        }

        public DbSet<PropertyModelEntity> Properties { get; set; }
        public DbSet<CountryDataEntity> CountryData { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
    }
}
