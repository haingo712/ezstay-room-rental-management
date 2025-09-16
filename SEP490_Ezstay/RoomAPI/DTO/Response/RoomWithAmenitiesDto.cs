using RoomAPI.DTO.Request;
using RoomAPI.Enum;

namespace RoomAPI.DTO.Response;

public class RoomWithAmenitiesDto
{
    public RoomDto Room { get; set; }
    public List<AmenityDto> Amenities { get; set; }
}
