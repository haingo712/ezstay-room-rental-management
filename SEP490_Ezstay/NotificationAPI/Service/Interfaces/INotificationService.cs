using AuthApi.Enums;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;

namespace NotificationAPI.Service.Interfaces
{
    public interface INotificationService 
    {
        Task<IEnumerable<NotificationResponseDto>> GetUserNotifications(Guid userId);
        Task<NotificationResponseDto> CreateAsync(CreateNotificationRequestDto dto);
        Task<IEnumerable<NotificationResponseDto>> GetAllNotifications();
        Task<NotificationResponseDto?> CreateRoleNoti(Guid id, RoleEnum role);
        Task<NotificationResponseDto?> UpdateNotifyByRole(Guid id, UpdateNotificationRequestDto dto, RoleEnum role);
        Task<bool> MarkAsRead(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
