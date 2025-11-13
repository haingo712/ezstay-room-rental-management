using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceAPI.Model
{
    public class ServiceModel
    {
        [BsonId] // định danh chính
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ServiceName { get; set; }
        public double Price { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
    