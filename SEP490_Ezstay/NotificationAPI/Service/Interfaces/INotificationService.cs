
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;
using Shared.Enums;

namespace NotificationAPI.Service.Interfaces
{
    public interface INotificationService 
    {
        //Task<List<NotificationResponseDto>> GetAllByUserAsync(Guid userId);

        // 🔹 Lấy chi tiết thông báo
        Task<NotificationResponseDto?> GetByIdAsync(Guid id);

        // 🔹 Tạo thông báo cho 1 user cụ thể
        Task<NotificationResponseDto> CreateAsync(Guid userId, NotifyRequest request);

        // 🔹 Cập nhật thông báo
        Task<NotificationResponseDto?> UpdateAsync(Guid id, NotifyRequest request);

        // 🔹 Xoá thông báo
        Task DeleteAsync(Guid id);

        // 🔹 Tạo thông báo cho 1 role
        Task<NotificationResponseDto> CreateByRoleAsync(NotifyByRoleRequest request);


        // 🔹 Đánh dấu thông báo đã đọc
        Task<bool> MarkAsReadAsync(Guid id);
        Task<NotificationResponseDto?> UpdateAsyncByRole(Guid id, NotifyRequest request);
        Task<List<NotificationResponseDto>> GetAllByUserAsync(Guid userId);
        Task CreateNotifyForOwnerRegisterAsync(Guid UserId, TriggerOwnerRegisterRequest dto);
        Task AproveNotifyForOwnerRegisterAsync(Guid UserId, TriggerOwnerRegisterRequest dto);
        Task RejectNotifyForOwnerRegisterAsync(Guid UserId, TriggerOwnerRegisterRequest dto);

        List<object> GetAllNotificationTypes();
        List<object> GetAllRoles();
        Task CreateNotifyAsync(NotifyByRoleRequest dto, Guid userId);




    }
}
