using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UtilityBillAPI.Models
{
    public class UtilityBillDetail
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UtilityBillId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid? UtilityReadingId { get; set; }
        public string Type { get; set; } = null!;
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal UnitPrice { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Consumption { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Total { get; set; }        
    }
}
