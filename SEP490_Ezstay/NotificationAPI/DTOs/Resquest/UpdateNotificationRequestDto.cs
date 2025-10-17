using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class UpdateNotificationRequestDto
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public NotificationType? NotificationType { get; set; }
    }
}
