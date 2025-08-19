using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BoardingHouseAPI.Models
{
    public class BoardingHouse
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid OwnerId { get; set; }
        public string HouseName { get; set; } = null!;
        public string? Description { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
