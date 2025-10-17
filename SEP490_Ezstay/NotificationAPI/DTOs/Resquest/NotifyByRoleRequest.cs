using AuthApi.Enums;
using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class NotifyByRoleRequest
    {

        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public RelatedItemType RelatedItemType { get; set; }
        public int RelatedItemId { get; set; }
        public RoleEnum TargetRole { get; set; }
    }
}
