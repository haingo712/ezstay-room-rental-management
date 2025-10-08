using System.ComponentModel.DataAnnotations;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.Enum;
using ContractAPI.Model;

namespace ContractAPI.DTO.Requests;

public class UpdateContractDto
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public DateTime CheckinDate { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Tiền cọc phải >= 0")]
    public decimal DepositAmount { get; set; }
    
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
    [Required]
    public List<CreateIdentityProfile> ProfilesInContract { get; set; } 
    [Required]
    public UpdateUtilityReading ElectricityReading { get; set; }
    [Required]
    public UpdateUtilityReading WaterReading { get; set; }
    
}