using System.ComponentModel.DataAnnotations;

namespace RoomAPI.DTO.Request;

public class CreateRoomDto
{
    [Required]
    public string RoomName { get; set; } 
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Area { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Price { get; set; }
}