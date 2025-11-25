using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums;

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
        public string? ServiceName { get; set; }
        public decimal? ServicePrice { get; set; }
        public string Type { get; set; } = null!;       

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Total { get; set; }        
    }
}
