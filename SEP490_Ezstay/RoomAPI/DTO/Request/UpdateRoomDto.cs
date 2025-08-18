using System.ComponentModel.DataAnnotations;

namespace RoomAPI.DTO.Request;

public class UpdateRoomDto
{
    public Guid HouseId { get; set; }
    //public int HouseLocationId { get; set; }
    public string RoomName { get; set; } 
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Area { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
}