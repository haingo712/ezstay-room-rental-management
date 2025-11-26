using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;
using Shared.Enums;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using Twilio.Http;
using static Google.Apis.Requests.BatchRequest;

namespace AuthApi.Services
{
    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly IAuthRepository _accountRepo;
        private readonly IOwnerRequestRepository _ownerRequestRepo;
        private readonly IMapper _mapper;
        private readonly string _NotifyApiUrl;
        private readonly string _UserApiUrl;
        private readonly IHttpClientFactory _httpClientFactory;


        public OwnerRequestService(
     IAuthRepository accountRepo,
     IOwnerRequestRepository ownerRequestRepo,
     IMapper mapper,
     IHttpClientFactory httpFactory,
     IConfiguration config)
        {
            _accountRepo = accountRepo;
            _ownerRequestRepo = ownerRequestRepo;
            _mapper = mapper;
            _httpClientFactory = httpFactory;
            _UserApiUrl = config["ApiSettings:UserApiUrl"]
        ?? throw new Exception("UserApiUrl not configured");

            _NotifyApiUrl = config["ApiSettings:NotifyApiUrl"]
                ?? throw new Exception("NotifyApiUrl not configured");

        }


        public async Task<OwnerRequestResponseDto?> SubmitRequestAsync(SubmitOwnerRequestDto dto, Guid accountId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // 1) GỌI USER API KIỂM TRA PROFILE
                var checkResp = await client.GetAsync($"{_UserApiUrl}/check-profile?id={accountId}");

                if (!checkResp.IsSuccessStatusCode)
                {
                    Console.WriteLine("❌ Không gọi được UserAPI check-profile");
                    return null;
                }

                var checkData = await checkResp.Content.ReadFromJsonAsync<Check>();

                if (checkData == null || checkData.IsValid == false)
                {
                    Console.WriteLine("❌ Profile chưa đầy đủ → không cho submit.");
                    return null;
                }

                // 2) TẠO REQUEST
                var entity = _mapper.Map<OwnerRegistrationRequest>(dto);
                entity.Id = Guid.NewGuid();
                entity.AccountId = accountId;
                entity.SubmittedAt = DateTime.UtcNow;
                entity.Status = RequestStatusEnum.Pending;

                await _ownerRequestRepo.CreateAsync(entity);

                var resultDto = _mapper.Map<OwnerRequestResponseDto>(entity);

                //// 3) GỬI NOTIFICATION
                var notifyPayload = new
                {
                    Title = "Yêu cầu đăng ký chủ trọ mới",
                    Message = $"User {accountId} vừa gửi đơn đăng ký chủ trọ.",
                    NotificationType = "OwnerRegister",
                    RequestId = entity.Id
                };
                Console.WriteLine($"❌ Submit request failed: {notifyPayload}");
                await client.PostAsJsonAsync($"{_NotifyApiUrl}/trigger-owner-register", notifyPayload);

                return resultDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Submit request failed: {ex.Message}");
                return null;
            }
        }






        public async Task<OwnerRequestResponseDto?> ApproveRequestAsync(Guid requestId, Guid staffId)
        {
            // Lấy request từ DB
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return null;

            // Cập nhật đơn
            request.Status = RequestStatusEnum.Approved;
            request.ReviewedByStaffId = staffId;
            request.ReviewedAt = DateTime.UtcNow;
            request.RejectionReason = null;

            await _ownerRequestRepo.UpdateAsync(request);

            // Gán role Owner cho account
            var account = await _accountRepo.GetByIdAsync(request.AccountId);
            if (account != null)
            {
                account.Role = Shared.Enums.RoleEnum.Owner;
                await _accountRepo.UpdateAsync(account);
            }

            // Map DTO trả về
            var responseDto = _mapper.Map<OwnerRequestResponseDto>(request);

            // -----------------------------------
            // 🔔 GỬI NOTIFICATION (ĐÃ DUYỆT)
            // -----------------------------------
            var notifyPayload = new
            {
                Title = "Yêu cầu đã được phê duyệt",
                Message = $"Đơn đăng ký chủ trọ của user {request.AccountId} đã được phê duyệt.",
                NotificationType = "OwnerApprove",
                RequestId = request.Id,
                StaffId = staffId
            };

            try
            {
                var client = _httpClientFactory.CreateClient();

                var notifyResponse = await client.PostAsJsonAsync(
                    $"{_NotifyApiUrl}/trigger-owner-approve",
                    notifyPayload
                );

                if (!notifyResponse.IsSuccessStatusCode)
                {
                    var body = await notifyResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Notification approve failed: {notifyResponse.StatusCode} - {body}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Approve notification error: {ex.Message}");
            }

            return responseDto;
        }


        public async Task<OwnerRequestResponseDto?> RejectRequestAsync(Guid requestId, Guid staffId, string rejectionReason)
        {
            // Lấy request từ DB
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return null;

            // Cập nhật trạng thái
            request.Status = RequestStatusEnum.Rejected;
            request.RejectionReason = rejectionReason;
            request.ReviewedByStaffId = staffId;
            request.ReviewedAt = DateTime.UtcNow;

            await _ownerRequestRepo.UpdateAsync(request);

            // Map DTO trả về
            var responseDto = _mapper.Map<OwnerRequestResponseDto>(request);

            // ---------------------------------------------------
            // 🔔 Gửi Notification (TỪ CHỐI)
            // ---------------------------------------------------
            var notifyPayload = new
            {
                Title = "Yêu cầu bị từ chối",
                Message = $"Đơn đăng ký chủ trọ của user {request.AccountId} đã bị từ chối. Lý do: {rejectionReason}",
                NotificationType = "OwnerReject",
                RequestId = request.Id,
                StaffId = staffId
            };

            try
            {
                var client = _httpClientFactory.CreateClient();

                var notifyResponse = await client.PostAsJsonAsync(
                    $"{_NotifyApiUrl}/trigger-owner-reject",
                    notifyPayload
                );

                if (!notifyResponse.IsSuccessStatusCode)
                {
                    var notifyBody = await notifyResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Notification reject failed: {notifyResponse.StatusCode} - {notifyBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Reject notification error: {ex.Message}");
            }

            return responseDto;
        }


        public async Task<List<OwnerRequestResponseDto>> GetAllRequestsAsync()
        {
            var allRequests = await _ownerRequestRepo.GetAllAsync();

            // ✅ Lọc chỉ lấy những đơn có trạng thái Pending
            var pendingRequests = allRequests
                .Where(r => r.Status == RequestStatusEnum.Pending)
                .ToList();

            return _mapper.Map<List<OwnerRequestResponseDto>>(pendingRequests);
        }

    }
}
