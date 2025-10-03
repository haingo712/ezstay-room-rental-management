using System.ComponentModel.DataAnnotations;

namespace RoomAmenityAPI.DTO.Response;

public class RoomAmenityResponseDto
{
    
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
    // public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}