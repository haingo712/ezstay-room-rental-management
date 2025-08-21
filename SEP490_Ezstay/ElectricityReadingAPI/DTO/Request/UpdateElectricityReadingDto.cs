using System.ComponentModel.DataAnnotations;

namespace ElectricityReadingAPI.DTO.Request;

public class UpdateElectricityReadingDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string AmenityName { get; set; } 
    [Required]
    public Guid OwnerId { get; set; }
}