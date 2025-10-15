using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NotificationAPI.Enums;
using AuthApi.Enums;

namespace NotificationAPI.Model
{
    public class Notify
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonElement("UserId")]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }


        public NotificationType NotificationType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public RelatedItemType RelatedItemType { get; set; }
        public int RelatedItemId { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public RoleEnum? TargetRole { get; set; }

    }
}
