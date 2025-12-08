using System.ComponentModel.DataAnnotations;
using RentalPostsAPI.Enum;
using Shared.DTOs.Amenities.Responses;

namespace RentalPostsAPI .DTO.Request;

public class RoomDto
{
    public Guid Id { get; set; }
    public Guid HouseId { get; set; }
    public string RoomName { get; set; } 
    public RoomStatus RoomStatus { get; set; }
    public decimal Area { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AmenityResponse> Amenities { get; set; }
}