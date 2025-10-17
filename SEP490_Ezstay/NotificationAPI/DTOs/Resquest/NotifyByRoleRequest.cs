using AuthApi.Enums;
using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class NotifyByRoleRequest
    {

        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
  
        public RoleEnum TargetRole { get; set; }
    

    }
}
