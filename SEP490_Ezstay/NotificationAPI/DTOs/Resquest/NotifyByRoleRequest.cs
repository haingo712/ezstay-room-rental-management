
using NotificationAPI.Enums;
using Shared.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class NotifyByRoleRequest
    {

        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public List<RoleEnum> TargetRoles { get; set; } = new();

        public DateTime? ScheduledTime { get; set; }


    }
}
