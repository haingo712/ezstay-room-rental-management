using System.ComponentModel.DataAnnotations;

namespace RentalRequestAPI.DTO.Request;

public class UpdateRentalRequestDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string AmenityName { get; set; } 
    [Required]
    public Guid StaffId { get; set; }
}