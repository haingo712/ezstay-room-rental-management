using System.ComponentModel.DataAnnotations;

namespace RoomAmenityAPI.DTO.Request;

public class RoomAmenityDto
{
    
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
}