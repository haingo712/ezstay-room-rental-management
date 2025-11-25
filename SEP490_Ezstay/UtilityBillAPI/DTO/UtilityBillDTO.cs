using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums;

namespace UtilityBillAPI.DTO
{
    public class UtilityBillDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid TenantId { get; set; }
        public Guid ContractId { get; set; }
        public Guid RoomId { get; set; }
        public decimal RoomPrice { get; set; }        
        public List<UtilityBillDetailDTO> Details { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UtilityBillType BillType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UtilityBillStatus Status { get; set; }
        public string? Note { get; set; }
        public string? Reason { get; set; }
    }
}
