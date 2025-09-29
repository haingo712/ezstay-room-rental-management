using System.ComponentModel.DataAnnotations;
using ContractAPI.DTO.Requests.UtilityReading;

namespace ContractAPI.DTO.Requests;

public class CreateContractDto
{
    public Guid TenantId { get; set; }
    [Required]
    public Guid RoomId { get; set; }

    [Required]
    public DateTime CheckinDate { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; }
  
    [Range(0, double.MaxValue, ErrorMessage = "Tiền cọc phải >= 0")]
    public decimal DepositAmount { get; set; }

    [Required]
    [Range(1, 9, ErrorMessage = "Number of occupants must be between 1 and 9.")]
    public int NumberOfOccupants { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
    [Required]
    public CreateIdentityProfileDto IdentityProfiles { get; set; } 
    // [Required]
    // public CreateUtilityReadingContract UtilityReadingContracts { get; set; } 
    [Required]
    public CreateUtilityReadingContract ElectricityReading { get; set; }
    [Required]
    public CreateUtilityReadingContract WaterReading { get; set; }
}