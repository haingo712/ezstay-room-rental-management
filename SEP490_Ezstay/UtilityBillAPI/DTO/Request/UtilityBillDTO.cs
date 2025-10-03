using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UtilityBillAPI.Enum;

namespace UtilityBillAPI.DTO.Request
{
    public class UtilityBillDTO
    {        
        public Guid Id { get; set; }        
        public Guid OwnerId { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoomId { get; set; }
        public Guid ElectricityId { get; set; }
        public Guid WaterId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /*public DateTime DueDate { get; set; }*/
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public UtilityBillStatus Status { get; set; }
        public string? Note { get; set; }        
    }
}
