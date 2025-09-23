using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FavoritePostAPI.Models
{
    public class FavoritePost
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid AccountId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
