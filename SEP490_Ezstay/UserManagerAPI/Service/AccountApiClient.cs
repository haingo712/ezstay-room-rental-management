using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
 // AccountRequest, UpdateAccountRequest, AccountResponse
using UserManagerAPI.Service.Interfaces;

namespace UserManagerAPI.Service
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _ownerRequestApiUrl;

        public AccountApiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
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


        public async Task<OwnerRequestResponseDto?> SubmitOwnerRequestAsync(SubmitOwnerRequestDto dto)
        {
            // POST thẳng JSON đến API, dùng JWT đã set trong HttpClient
            var response = await _http.PostAsJsonAsync(_ownerRequestApiUrl, dto);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to submit owner request. StatusCode: {response.StatusCode}");
                return null;
            }

            // Deserialize thẳng sang DTO
            return await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();
        }


        public async Task<OwnerRequestResponseDto?> ApproveOwnerRequestAsync(Guid requestId)
        {
            var response = await _http.PutAsync($"{_ownerRequestApiUrl}/approve/{requestId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to approve owner request. StatusCode: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();
        }

        public async Task<OwnerRequestResponseDto?> RejectOwnerRequestAsync(Guid requestId, string rejectionReason)
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

            return await response.Content.ReadFromJsonAsync<OwnerRequestResponseDto>();
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

