using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NotificationAPI.Enums;
using Shared.Enums;


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


        public NotificationType NotificationType { get; set; } // loại thông báo
        public string Title { get; set; }
        public string Message { get; set; }


        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<RoleEnum> TargetRoles { get; set; } = new();



        public DateTime? ScheduledTime { get; set; } // thời gian hẹn gửi
        public bool IsSent { get; set; } = false;    // đã gửi hay chưa


    }
}
