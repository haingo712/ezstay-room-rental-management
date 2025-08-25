using System.ComponentModel.DataAnnotations;

namespace AmenityAPI.DTO.Request;

public class AmenityDto
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } 
    public Guid StaffId { get; set; }
}