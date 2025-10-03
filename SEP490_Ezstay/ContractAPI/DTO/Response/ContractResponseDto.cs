
using System.ComponentModel.DataAnnotations;
using ContractAPI.Enum;

namespace ContractAPI.DTO.Response;

public class ContractResponseDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }
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
    public IdentityProfileResponseDto IdentityProfiles { get; set; } 
}
