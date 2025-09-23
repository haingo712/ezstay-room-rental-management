using System.ComponentModel.DataAnnotations;

namespace AmenityAPI.DTO.Response;

public class AmenityResponseDto
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } 
    //public Guid StaffId { get; set; }
     public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}