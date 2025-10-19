    using APIGateway.Helper.Interfaces;
    using AuthApi.DTO.Request;
    using AuthApi.DTO.Response;
    using AuthApi.Enums;
    using AuthApi.Models;
    using AutoMapper;
    using MongoDB.Driver;
    using NotificationAPI.DTOs.Respone;
    using NotificationAPI.DTOs.Resquest;
    using NotificationAPI.Enums;
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
                _ = StartScheduleMonitorAsync();
            }

        //public async Task<List<NotificationResponseDto>> GetAllByUserAsync(Guid userId)
        //{
        //    var list = await _repo.GetByUserIdAsync(userId);
        //    return _mapper.Map<List<NotificationResponseDto>>(list);
        //}


        private async Task StartScheduleMonitorAsync()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var now = DateTime.UtcNow;

                    // 🔎 Chỉ lấy những notify chưa gửi mà đã đến giờ
                    var filter = Builders<Notify>.Filter.And(
                        Builders<Notify>.Filter.Eq(n => n.IsSent, false),
                        Builders<Notify>.Filter.Ne(n => n.ScheduledTime, null),
                        Builders<Notify>.Filter.Lte(n => n.ScheduledTime, now)
                    );

                    var update = Builders<Notify>.Update
                        .Set(n => n.IsSent, true);

                    var result = await _repo.UpdateManyAsync(filter, update);

                    if (result.ModifiedCount > 0)
                    {
                        Console.WriteLine($"[Scheduler] Đã gửi {result.ModifiedCount} thông báo hẹn giờ lúc {DateTime.Now}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(30)); // ⏱ kiểm tra mỗi 30 giây
                }
            });
        }


        public async Task<NotificationResponseDto?> GetByIdAsync(Guid id)
            {
                var notify = await _repo.GetByIdAsync(id);
                return notify == null ? null : _mapper.Map<NotificationResponseDto>(notify);
            }


            public List<object> GetAllNotificationTypes()
            {
                return _repo.GetAllNotificationTypes();
            }

            public List<object> GetAllRoles()
            {
                return _repo.GetAllRoles();
            }

            public async Task<NotificationResponseDto> CreateAsync(Guid userId, NotifyRequest request)
            {
                var entity = new Notify
                {
                    UserId = userId,
                    NotificationType = request.NotificationType,
                    Title = request.Title,
                    Message = request.Message,

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

            // ⚙️ Chỉ lấy những notify đã gửi hoặc không hẹn giờ
            var now = DateTime.UtcNow;
            var filtered = list.Where(n =>
                n.ScheduledTime == null ||
                (n.ScheduledTime <= now && n.IsSent == true)
            ).ToList();

            return _mapper.Map<List<NotificationResponseDto>>(filtered);
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


            public async Task CreateNotifyForOwnerRegisterAsync(Guid UserId,TriggerOwnerRegisterRequest dto)
            {
                var notify = new Notify
                {
                    Title = "Yêu cầu đăng ký chủ trọ mới",
                    Message = "Một người dùng vừa gửi đơn đăng ký làm chủ trọ. Vui lòng kiểm tra.",
                    NotificationType = NotificationType.OwnerRegister,
                    UserId = UserId, // không gán cho user cụ thể
                    TargetRole = RoleEnum.Staff,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(notify); // gọi đúng method lưu vào MongoDB
            }


            public async Task AproveNotifyForOwnerRegisterAsync(Guid UserId, TriggerOwnerRegisterRequest dto)
            {
                var notify = new Notify
                {
                    Title = "Yêu cầu đănng ký của bạn đã được duyệt",
                    Message = "xin chúc mừng bạn tài khoản bạn đã được nâng cấp",
                    NotificationType = NotificationType.OwnerRegister,

                    UserId = UserId, // không gán cho user cụ thể
                    TargetRole = RoleEnum.User,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(notify); // gọi đúng method lưu vào MongoDB
            }

            public async Task RejectNotifyForOwnerRegisterAsync(Guid UserId, TriggerOwnerRegisterRequest dto)
            {
                var notify = new Notify
                {
                    Title = "Yêu cầu đănng ký của bạn đã bị từ chối",
                    Message = "Xin lôi vì một số lý do nên tài khoản của bạn không được chấp nhận",
                    NotificationType = NotificationType.OwnerRegister,

                    UserId = UserId, // không gán cho user cụ thể
                    TargetRole = RoleEnum.User,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(notify); // gọi đúng method lưu vào MongoDB
            }


        public async Task CreateNotifyAsync(NotifyByRoleRequest dto, Guid userId)
        {
            if (dto.ScheduledTime.HasValue && dto.ScheduledTime.Value <= DateTime.UtcNow)
                throw new Exception("Thời gian hẹn phải lớn hơn thời gian hiện tại.");

            var notify = new Notify
            {
                UserId = userId,
                NotificationType = dto.NotificationType,
                Title = dto.Title,
                Message = dto.Message,
                TargetRole = dto.TargetRole,
                ScheduledTime = dto.ScheduledTime,
                CreatedAt = DateTime.UtcNow,
                IsSent = !dto.ScheduledTime.HasValue, // 👈 nếu không hẹn giờ → gửi luôn
                IsRead = false
            };

            await _repo.CreateAsync(notify);
        }




        public async Task<List<Notify>> GetDueNotifiesAsync()
            {
                return await _repo.GetDueNotifiesAsync();
            }

            public async Task MarkAsSentAsync(Guid id)
            {
                await _repo.MarkAsSentAsync(id);
            }


        }
    }
