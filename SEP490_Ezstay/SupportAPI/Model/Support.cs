using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SupportAPI.Enums;

namespace SupportAPI.Model
{
    public class Support
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid id = Guid.NewGuid();
        public string Subject { get; set; }
        public string Description { get; set; }
        public StatusEnums status { get; set; } = StatusEnums.Pending;
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
