using System.ComponentModel.DataAnnotations;

namespace AmenityAPI.Models;

public class Amenity
{
    [Key]
    public int AmenityId { get; set; }
    public Guid OwnerId { get; set; }
    public string AmenityName { get; set; } = null!;
}
