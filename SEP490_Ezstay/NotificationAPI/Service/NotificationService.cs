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

        public NotificationService(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
