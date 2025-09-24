
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
    [Required]
    [Range(1, 9, ErrorMessage = "Number of occupants must be between 1 and 9.")]
    public int NumberOfOccupants { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}
