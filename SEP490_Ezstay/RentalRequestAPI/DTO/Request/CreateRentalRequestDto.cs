using System.ComponentModel.DataAnnotations;
using RentalRequestAPI.Enum;

namespace RentalRequestAPI.DTO.Request;

public class CreateRentalRequestDto
{
    public Guid RoomId { get; set; }
    public string? FullName { get; set; }
    [Required]
    [Phone]
    public string NumberPhone { get; set; }
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string? Notes { get; set; }
}