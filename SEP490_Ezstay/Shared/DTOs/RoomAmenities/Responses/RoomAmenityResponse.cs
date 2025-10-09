using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.RoomAmenities.Responses;

public class RoomAmenityResponse
{
    
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
    // public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}