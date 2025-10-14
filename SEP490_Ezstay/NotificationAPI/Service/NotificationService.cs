using AuthApi.Enums;
using AutoMapper;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Model;
using NotificationAPI.Repositories.Interfaces;
using NotificationAPI.Service.Interfaces;
using Twilio.Rest.Conversations.V1.Service.Configuration;

namespace NotificationAPI.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;
        private readonly INotificationSender _notificationSender;

        public NotificationService(INotificationRepository repo, IMapper mapper, INotificationSender notificationSender)
        {
            _repo = repo;
            _mapper = mapper;
            _notificationSender = notificationSender;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetUserNotifications(Guid userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<NotificationResponseDto>>(list);
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotifications()
        {
            var list = await _repo.GetAllAsync(); // ➕ gọi repository
            return _mapper.Map<IEnumerable<NotificationResponseDto>>(list);
        }


        public async Task<NotificationResponseDto> CreateAsync(CreateNotificationRequestDto dto)
        {
            var notify = _mapper.Map<Notify>(dto);
            await _repo.AddAsync(notify);

            var notifyDto = _mapper.Map<NotificationResponseDto>(notify);
            // 🔔 Gửi real-time notification
            await _notificationSender.SendToAllAsync($"📢 {notifyDto.Title}: {notifyDto.Message}");

            return notifyDto;
        }

        public async Task<NotificationResponseDto?> CreateNotifyByRoleAsync(CreateNotificationRequestDto dto, RoleEnum role)
        {
            // 🔹 B1: map DTO → entity
            var notify = _mapper.Map<Notify>(dto);

            // Tùy chọn: thêm Role để biết thông báo này gửi cho role nào
            // Nếu bạn chưa có trường Role trong Notify, có thể bỏ dòng này
            // notify.Role = role;

            await _repo.AddAsync(notify);

            // 🔹 B2: lấy danh sách account theo role từ AuthAPI qua Gateway
            var accounts = await _notificationSender.GetByRoleAsync(role);
            if (accounts == null || !accounts.Any()) return null;

            // 🔹 B3: gửi notification tới tất cả user thuộc role đó
            foreach (var acc in accounts)
            {
                await _notificationSender.SendToAllAsync($"🔔 {notify.Title}: {notify.Message}");
            }

            // 🔹 B4: trả về DTO kết quả
            return _mapper.Map<NotificationResponseDto>(notify);
        }


        public async Task<NotificationResponseDto?> UpdateNotifyByRole(Guid id,UpdateNotificationRequestDto dto, RoleEnum role)
        {
            // 🔹 Lấy notify cần cập nhật
            var notify = await _repo.GetByIdAsync(id);
            if (notify == null) return null;

            // 🔹 Map dữ liệu từ DTO sang entity (AutoMapper)
            _mapper.Map(dto, notify);
            notify.IsRead = false;
            notify.CreatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(notify);

            // 🔹 Lấy danh sách account theo Role
            var accounts = await _notificationSender.GetByRoleAsync(role);
            if (accounts == null || !accounts.Any()) return null;

            // 🔹 Gửi notify cập nhật tới từng user theo role
            foreach (var acc in accounts)
            {
                await _notificationSender.SendToAllAsync($"♻️ [Cập nhật] {notify.Title}: {notify.Message}");
            }

            return _mapper.Map<NotificationResponseDto>(notify);
        }






        public async Task<bool> MarkAsRead(Guid id)
        {
            var notify = await _repo.GetByIdAsync(id);
            if (notify == null) return false;

            notify.IsRead = true;
            await _repo.UpdateAsync(notify);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var notify = await _repo.GetByIdAsync(id);
            if (notify == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
