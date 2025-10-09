
using System.ComponentModel.DataAnnotations;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;


namespace Shared.DTOs.Contracts.Responses;

public class ContractResponse
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    // public Guid TenantId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CanceledAt { get; set; }
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public ContractStatus ContractStatus { get; set; }
    public string Reason { get; set; }
    public decimal DepositAmount { get; set; }
    public int NumberOfOccupants { get; set; }
    public string? Notes { get; set; }
    
    public List<IdentityProfileResponse> IdentityProfiles { get; set; } 
    public UtilityReadingResponse ElectricityReading { get; set; }
    public UtilityReadingResponse WaterReading { get; set; }
}
