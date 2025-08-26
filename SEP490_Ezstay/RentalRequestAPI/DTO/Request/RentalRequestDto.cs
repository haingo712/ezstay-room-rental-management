using System.ComponentModel.DataAnnotations;

namespace RentalRequestAPI.DTO.Request;

public class RentalRequestDto
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } 
    public Guid StaffId { get; set; }
}