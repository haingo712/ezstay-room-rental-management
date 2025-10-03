using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UtilityBillAPI.Enum;

namespace UtilityBillAPI.Models
{
    public class UtilityBill
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid TenantId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid RoomId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid ElectricityId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid WaterId { get; set; }
        public decimal Amount { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? PaymentDate { get; set; }
        /*[BsonRepresentation(BsonType.DateTime)]
        public DateTime DueDate { get; set; }   */
        public string? PaymentMethod { get; set; }
        [BsonRepresentation(BsonType.String)]
        public UtilityBillStatus Status { get; set; }
        public string? Note { get; set; }        

    }
}
