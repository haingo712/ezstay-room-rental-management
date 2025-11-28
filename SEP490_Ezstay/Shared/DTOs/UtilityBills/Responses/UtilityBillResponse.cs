using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.UtilityBills.Responses;

public class UtilityBillResponse
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }
    public Guid ContractId { get; set; }
    public string HouseName { get; set; } = null!;
    public string RoomName { get; set; } = null!;
    public decimal? RoomPrice { get; set; }
    public List<UtilityBillDetailResponse> Details { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityBillType BillType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityBillStatus Status { get; set; }
    public string? Note { get; set; }
    public string? Reason { get; set; }
}