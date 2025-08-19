using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseLocationAPI.Models
{
    public class HouseLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid HouseId { get; set; }
        public string FullAddress { get; set; } = null!;

    }
}
