using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;

// AccountRequest, UpdateAccountRequest, AccountResponse
using UserManagerAPI.Service.Interfaces;

namespace UserManagerAPI.Service
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _ownerRequestApiUrl;
        private readonly string _NotifyApiUrl;


        public AccountApiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _NotifyApiUrl = config["ApiSettings:NotifyApiUrl"]
      ?? throw new Exception("NotifyApiUrl not configured");

            _baseUrl = config["ApiSettings:AccountApiBaseUrl"]
                           ?? throw new Exception("AccountApiBaseUrl not configured");
            _ownerRequestApiUrl = config["ApiSettings:OwnerRequestApiUrl"]
         ?? throw new Exception("OwnerRequestApiUrl not configured");
        }

        // Set token trước khi gọi API
        public void SetJwtToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<AccountResponse>?> GetAllAsync()
        {
            var response = await _http.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AccountResponse>>();
        }

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"{_baseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        public async Task<AccountResponse?> CreateAsync(AccountRequest request)
        {
            var response = await _http.PostAsJsonAsync(_baseUrl, request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        public async Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request)
        {
            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        public async Task BanAsync(Guid id)
        {
            var response = await _http.PatchAsync($"{_baseUrl}/{id}/ban", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnbanAsync(Guid id)
        {
            var response = await _http.PatchAsync($"{_baseUrl}/{id}/unban", null);
            response.EnsureSuccessStatusCode();
        }


        public async Task<OwnerRequestResponseDto?> SubmitOwnerRequestAsync(SubmitOwnerRequestClientDto dto, Guid accountId)
        {
            // Payload gửi AuthAPI
            var payload = new
            {
                Reason = dto.Reason,
                AccountId = accountId
            };

            var response = await _http.PostAsJsonAsync($"{_ownerRequestApiUrl}/request-owner", payload);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to submit owner request. StatusCode: {response.StatusCode}, Content: {content}");
                return null;
            }

            var resultDto = await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();

            // ✅ Gửi notification nếu submit thành công
            if (resultDto != null)
            {
                var notifyPayload = new
                {
                    Title = "Yêu cầu đăng ký chủ trọ mới",
                    Message = $"User {accountId} vừa gửi đơn đăng ký làm chủ trọ. Vui lòng kiểm tra.",
                    NotificationType = "OwnerRegister",
                };

                var notifyResponse = await _http.PostAsJsonAsync($"{_NotifyApiUrl}/trigger-owner-register", notifyPayload);
                if (!notifyResponse.IsSuccessStatusCode)
                {
                    var notifyContent = await notifyResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to create notification. StatusCode: {notifyResponse.StatusCode}, Content: {notifyContent}");
                }
            }

            return resultDto;
        }




        public async Task<OwnerRequestResponseDto?> ApproveOwnerRequestAsync(Guid requestId, Guid accountId)
        {
            var response = await _http.PutAsync($"{_ownerRequestApiUrl}/approve/{requestId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to approve owner request. StatusCode: {response.StatusCode}");
                return null;
            }

            var resultDto = await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();
            if (resultDto != null)
            {
                var notifyPayload = new
                {
                    Title = "Yêu cầu đăng ký chủ trọ mới",
                    Message = $"User {accountId} vừa gửi đơn đăng ký làm chủ trọ. Vui lòng kiểm tra.",
                    NotificationType = "OwnerRegister"
                };

                var notifyResponse = await _http.PostAsJsonAsync($"{_NotifyApiUrl}/triger-aprove-Owner", notifyPayload);
                if (!notifyResponse.IsSuccessStatusCode)
                {
                    var notifyContent = await notifyResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to create notification. StatusCode: {notifyResponse.StatusCode}, Content: {notifyContent}");
                }
            }

            return resultDto;
        }

        public async Task<OwnerRequestResponseDto?> RejectOwnerRequestAsync(Guid requestId, string rejectionReason,Guid accountId)
        {
            var payload = new RejectOwnerRequestDto
            {
                RejectionReason = rejectionReason
            };

            var response = await _http.PutAsJsonAsync($"{_ownerRequestApiUrl}/reject/{requestId}", payload);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to reject owner request. StatusCode: {response.StatusCode}");
                return null;
            }
            var resultDto = await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();
            
                var notifyResponse = await _http.PostAsJsonAsync($"{_NotifyApiUrl}/triger-reject-Owner", null);
                if (!notifyResponse.IsSuccessStatusCode)
                {
                   await notifyResponse.Content.ReadAsStringAsync();
                   
                }
            
            return resultDto;


        }

        public async Task<List<OwnerRequestResponseDto>?> GetPendingRequestsForStaffAsync()
        {
            var response = await _http.GetAsync(_ownerRequestApiUrl); // Gọi /api/ownerrequest
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<OwnerRequestResponseDto>>();
        }




    }









}

