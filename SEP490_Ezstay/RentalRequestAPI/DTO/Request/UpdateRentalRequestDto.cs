using System.ComponentModel.DataAnnotations;

namespace RentalRequestAPI.DTO.Request;

public class UpdateRentalRequestDto
{
    public string? FullName { get; set; }
    [Required]
    [Phone]
    public string NumberPhone { get; set; }
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string? Notes { get; set; }
}