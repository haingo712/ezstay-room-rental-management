using APIGateway.Helper.Interfaces;
using AuthApi.DTO.Response;
using AuthApi.Enums;
using AutoMapper;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Model;
using NotificationAPI.Repositories.Interfaces;
using NotificationAPI.Service.Interfaces;
using System.Data;
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

        //public async Task<List<NotificationResponseDto>> GetAllByUserAsync(Guid userId)
        //{
        //    var list = await _repo.GetByUserIdAsync(userId);
        //    return _mapper.Map<List<NotificationResponseDto>>(list);
        //}

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

        public async Task<List<NotificationResponseDto>> GetAllByRoleOrUserAsync(Guid userId, RoleEnum role)
        {
            var list = await _repo.GetAllForRoleOrUserAsync(userId, role);
            return _mapper.Map<List<NotificationResponseDto>>(list);
        }


        public async Task<NotificationResponseDto> CreateByRoleAsync(NotifyByRoleRequest request)
        {
            // Gọi qua Auth API để lấy danh sách user thuộc role
            var usersResponse = await _httpClient.GetFromJsonAsync<List<AccountResponse>>(
                $"api/accounts/role/{(int)request.TargetRole}");

            if (usersResponse == null || !usersResponse.Any())
                throw new Exception($"Không tìm thấy user nào thuộc role {request.TargetRole}");

            // Tạo thông báo (chỉ một bản, dành cho role)
            var notify = new Notify
            {
                NotificationType = request.NotificationType,
                Title = request.Title,
                Message = request.Message,
                RelatedItemType = request.RelatedItemType,
                RelatedItemId = request.RelatedItemId,
                CreatedAt = DateTime.UtcNow,

                // 👇 thêm dòng này để lưu role, giúp hiển thị hoặc lọc về sau
                TargetRole = request.TargetRole
            };

            // Lưu vào MongoDB
            await _repo.AddAsync(notify);

            // Map sang DTO trả về
            var response = _mapper.Map<NotificationResponseDto>(notify);
            return response;
        }





        public async Task<NotificationResponseDto?> UpdateAsyncByRole(Guid id, NotifyRequest request)
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









        // 🟢 Đánh dấu đã đọc
        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var exist = await _repo.GetByIdAsync(id);
            if (exist == null) return false;

            return await _repo.MarkAsReadAsync(id);
        }


    }
}
