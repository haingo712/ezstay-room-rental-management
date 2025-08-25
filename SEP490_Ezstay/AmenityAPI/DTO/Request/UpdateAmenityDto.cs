using System.ComponentModel.DataAnnotations;

namespace AmenityAPI.DTO.Request;

public class UpdateAmenityDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string AmenityName { get; set; } 
  //  [Required]
  //  public Guid StaffId { get; set; }
}