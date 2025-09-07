using System.ComponentModel.DataAnnotations;
using TenantAPI.Enum;

namespace TenantAPI.DTO.Requests;

public class CreateTenantDto {
  
  public Guid UserId { get; set; }
  [Required]
  public Guid RoomId { get; set; }
  [Required]
  public DateTime CheckinDate { get; set; }
  [Required]
  public DateTime CheckoutDate { get; set; }
  [Required]
  public TenantStatus  TenantStatus{ get; set; }
  [Required]
  [Range(1, 9, ErrorMessage = "Number of occupants must be between 1 and 9.")]
  public int NumberOfOccupants { get; set; }
  [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
  public string? Notes { get; set; }

}