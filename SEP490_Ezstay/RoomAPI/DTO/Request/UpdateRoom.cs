using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace RoomAPI.DTO.Request;

public class UpdateRoom
{
     [Required]
    public string RoomName { get; set; } 
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Area { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    [Required]
    public decimal Price { get; set; }
    [Required]
    public RoomStatus RoomStatus { get; set; }
    [Required]
    public IFormFile ImageUrl { get; set;}
}