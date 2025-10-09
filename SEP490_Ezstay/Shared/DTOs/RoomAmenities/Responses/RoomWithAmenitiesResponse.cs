using Shared.DTOs.Amenities.Responses;
using Shared.DTOs.Rooms.Responses;

namespace Shared.DTOs.RoomAmenities.Responses;

public class RoomWithAmenitiesResponse
{
    public RoomResponse Room { get; set; }
    public List<AmenityResponse> Amenities { get; set; }
}