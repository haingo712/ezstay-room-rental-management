using System.ComponentModel.DataAnnotations;

namespace RoomAPI.DTO.Request;

public class CreateRoom
{
    [Required]
    public string RoomName { get; set; } 
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Area { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Price { get; set; }
    [Required]
    public IFormFileCollection ImageUrl { get; set;}
    
    public List<Guid>? AmenityIds { get; set; }
}