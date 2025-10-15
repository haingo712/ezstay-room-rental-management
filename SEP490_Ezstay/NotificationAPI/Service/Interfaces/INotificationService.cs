using AuthApi.Enums;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;

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
        Task<List<NotificationResponseDto>> GetAllByRoleOrUserAsync(Guid userId, RoleEnum role);


        }
}
