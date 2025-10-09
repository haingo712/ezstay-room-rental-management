using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Amenities.Responses;

public class AmenityResponse
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } 
    //public Guid StaffId { get; set; }
     public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}