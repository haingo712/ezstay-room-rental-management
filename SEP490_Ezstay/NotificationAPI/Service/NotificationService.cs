using APIGateway.Helper.Interfaces;
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
        private readonly HttpClient _httpClient;
        private readonly IUserClaimHelper _userHelper;

        public NotificationService(
              INotificationRepository repo,
              IMapper mapper,
              IUserClaimHelper userHelper,
              IHttpClientFactory httpFactory)
        {
            _repo = repo;
            _mapper = mapper;
            _userHelper = userHelper;
            _httpClient = httpFactory.CreateClient("Gateway");
        }

        public async Task<List<NotificationResponseDto>> GetAllByUserAsync(Guid userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<List<NotificationResponseDto>>(list);
        }

        public async Task<NotificationResponseDto?> GetByIdAsync(Guid id)
        {
            var notify = await _repo.GetByIdAsync(id);
            return notify == null ? null : _mapper.Map<NotificationResponseDto>(notify);
        }

        public async Task<NotificationResponseDto> CreateAsync(Guid userId, NotifyRequest request)
        {
            var entity = new Notify
            {
                UserId = userId,
                NotificationType = request.NotificationType,
                Title = request.Title,
                Message = request.Message,
                RelatedItemType = request.RelatedItemType,
                RelatedItemId = request.RelatedItemId,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            return _mapper.Map<NotificationResponseDto>(entity);
        }

        public async Task<NotificationResponseDto?> UpdateAsync(Guid id, NotifyRequest request)
        {
            var notify = await _repo.GetByIdAsync(id);
            if (notify == null) return null;

            notify.Title = request.Title;
            notify.Message = request.Message;
            notify.NotificationType = request.NotificationType;
            notify.RelatedItemType = request.RelatedItemType;
            notify.RelatedItemId = request.RelatedItemId;

            await _repo.UpdateAsync(notify);
            return _mapper.Map<NotificationResponseDto>(notify);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }


        public async Task<List<NotificationResponseDto>> CreateByRoleAsync(NotifyByRoleRequest request)
        {
            var users = await _httpClient.GetFromJsonAsync<List<Guid>>(
                $"api/account/get-by-role?role={request.TargetRole}");

            if (users == null || !users.Any())
                throw new Exception($"Không tìm thấy user nào thuộc role {request.TargetRole}");

            var listNotify = users.Select(userId => new Notify
            {
                UserId = userId,
                NotificationType = request.NotificationType,
                Title = request.Title,
                Message = request.Message,
                RelatedItemType = request.RelatedItemType,
                RelatedItemId = request.RelatedItemId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _repo.CreateManyAsync(listNotify);
            return _mapper.Map<List<NotificationResponseDto>>(listNotify);
        }

        public async Task<List<NotificationResponseDto>> UpdateByRoleAsync(NotifyByRoleRequest request)
        {
            var users = await _httpClient.GetFromJsonAsync<List<Guid>>(
                $"api/account/get-by-role?role={request.TargetRole}");

            if (users == null || !users.Any())
                throw new Exception($"Không tìm thấy user nào thuộc role {request.TargetRole}");

            var notifies = await _repo.GetByUserIdsAsync(users);

            foreach (var notify in notifies)
            {
                notify.NotificationType = request.NotificationType;
                notify.Title = request.Title;
                notify.Message = request.Message;
                notify.RelatedItemType = request.RelatedItemType;
                notify.RelatedItemId = request.RelatedItemId;
                notify.CreatedAt = DateTime.UtcNow;
            }

            await _repo.UpdateManyAsync(notifies);
            return _mapper.Map<List<NotificationResponseDto>>(notifies);
        }








        // 🟢 Đánh dấu đã đọc
        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var exist = await _repo.GetByIdAsync(id);
            if (exist == null) return false;

            return await _repo.MarkAsReadAsync(id);
        }


    }
}
