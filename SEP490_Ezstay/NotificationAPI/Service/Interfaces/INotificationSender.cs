using AuthApi.Enums;

namespace NotificationAPI.Service.Interfaces
{
    public interface INotificationSender
    {
        Task SendToAllAsync(string message);
        Task<List<object>> GetByRoleAsync(RoleEnum role);
    }
}
