using System.ComponentModel.DataAnnotations;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.Enum;
using ContractAPI.Model;

namespace ContractAPI.DTO.Requests;

public class UpdateContractDto
{
    //public Guid TenantId { get; set; }
    public Guid SignerProfileId { get; set; } 
       
    public IdentityProfile SignerProfile { get; set; }
        
    public List<IdentityProfile> OccupantProfiles { get; set; }
    
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    
    [Required]
    [Range(1, 9, ErrorMessage = "Number of occupants must be between 1 and 9.")]
    public int NumberOfOccupants { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
    [Required]
    public CreateIdentityProfile CreateIdentityProfile { get; set; }
    [Required]
    public CreateUtilityReadingContract ElectricityReading { get; set; }
    [Required]
    public CreateUtilityReadingContract WaterReading { get; set; }
}