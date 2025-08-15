using System.ComponentModel.DataAnnotations;

namespace AmenityAPI.DTO.Request;

public class AmenityDto
{
    [Key]
    public int AmenityId { get; set; }
    public string AmenityName { get; set; } 
    public Guid OwnerId { get; set; }
}