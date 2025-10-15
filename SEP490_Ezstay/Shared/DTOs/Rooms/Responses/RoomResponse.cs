using Shared.Enums;

namespace Shared.DTOs.Rooms.Responses;

public class RoomResponse
{
    public Guid Id { get; set; }
  
    public Guid HouseId { get; set; }
    public string Image { get; set;}
    public string RoomName { get; set;} 
    public decimal Area { get; set; }
    public decimal Price { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
