using Shared.DTOs.Amenities.Responses;
using Shared.DTOs.RoomAmenities.Responses;
using Shared.Enums;

namespace Shared.DTOs.Rooms.Responses;

public class RoomResponse
{
    public Guid Id { get; set; }
  
    public Guid HouseId { get; set; }
    public List<string> ImageUrl { get; set;}
    public string RoomName { get; set;} 
    public decimal Area { get; set; }
    public decimal Price { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AmenityResponse> Amenities { get; set; }
}
