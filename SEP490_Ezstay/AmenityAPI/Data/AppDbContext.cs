


using AmenityAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace AmenityAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

      
        public DbSet<Amenity> Amenities { get; set; }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { AmenityId = 1, AmenityName = "WiFi",  OwnerId = Guid.Parse("e8447baa-b99d-41ab-a0d7-ddaee80e2ff3")},
                new Amenity { AmenityId = 2, AmenityName = "Air Conditioning", OwnerId = Guid.Parse("e8447baa-b99d-41ab-a0d7-ddaee80e2ff3")},
                new Amenity { AmenityId = 3, AmenityName = "Heating",  OwnerId = Guid.Parse("5b8d129c-414d-4035-9c42-d38d733c8305") },
                new Amenity { AmenityId = 4, AmenityName = "Parking", OwnerId = Guid.Parse("5b8d129c-414d-4035-9c42-d38d733c8305")},
                new Amenity { AmenityId = 5, AmenityName = "Laundry", OwnerId = Guid.Parse("5b8d129c-414d-4035-9c42-d38d733c8305") }
            );
        }
    }
}

