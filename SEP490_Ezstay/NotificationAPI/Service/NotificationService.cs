using AuthApi.Enums;
using AutoMapper;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Model;
using NotificationAPI.Repositories.Interfaces;
using NotificationAPI.Service.Interfaces;

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

        public async Task<NotificationResponseDto?> CreateRoleNoti(Guid id, RoleEnum role)
        {
            // Lấy notify từ repo
            var notify = await _repo.GetByIdAsync(id);
            if (notify == null) return null;

            // 🔹 Lấy danh sách account theo role (gọi qua Gateway → AuthAPI)
            var accounts = await _notificationSender.GetByRoleAsync(role);
            if (accounts == null || !accounts.Any()) return null;

            // 🔹 Gửi notify đến từng account theo role
            foreach (var acc in accounts)
            {
                await _notificationSender.SendToAllAsync($"🔔 {notify.Title}: {notify.Message}");
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
