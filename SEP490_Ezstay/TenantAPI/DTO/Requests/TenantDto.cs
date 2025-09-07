using System.ComponentModel.DataAnnotations;
using TenantAPI.Enum;
using TenantAPI.Model;

namespace TenantAPI.DTO.Requests;

public class TenantDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public TenantStatus  TenantStatus{ get; set; }
    [Required]
    [Range(1, 9, ErrorMessage = "Number of occupants must be between 1 and 9.")]
    public int NumberOfOccupants { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}


