using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceAPI.Model
{
    public class ServiceItem
    {
        [BsonId] // định danh chính
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ServiceName { get; set; }
        public decimal Price { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [BsonRepresentation(BsonType.String)]
        public Guid OwnerId { get; set; }
    }
}
    