using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Respone
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public RelatedItemType RelatedItemType { get; set; }
        public int RelatedItemId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
