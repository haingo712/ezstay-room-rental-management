using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class CreateNotificationRequestDto
    {
        public Guid UserId { get; set; } 
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public RelatedItemType RelatedItemType { get; set; }
        public int RelatedItemId { get; set; }
    }
}
