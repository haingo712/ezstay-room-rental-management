using System.ComponentModel.DataAnnotations;

namespace RoomAPI.DTO.Request;

public class RoomDto
{
    [Key]
    public Guid Id { get; set; }
    public Guid HouseId { get; set; }
    public Guid HouseLocationId { get; set; }
    public string RoomName { get; set; } 
    public decimal Area { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
}