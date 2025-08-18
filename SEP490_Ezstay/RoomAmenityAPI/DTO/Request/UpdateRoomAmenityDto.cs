using System.ComponentModel.DataAnnotations;

namespace RoomAmenityAPI.DTO.Request;

public class UpdateRoomAmenityDto
{
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
    
}