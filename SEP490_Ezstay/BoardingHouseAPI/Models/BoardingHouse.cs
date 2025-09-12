using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BoardingHouseAPI.Models
{
    public class BoardingHouse
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid HouseLocationId { get; set; }
        public string HouseName { get; set; } = null!;
        public string? Description { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public HouseLocation? Location { get; set; }
    }
}
