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

        public AccountApiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _baseUrl = config["ApiSettings:AccountApiBaseUrl"]
                       ?? throw new Exception("AccountApiBaseUrl not configured");
        }

        // GET ALL
        public async Task<List<AccountResponse>?> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<AccountResponse>>(_baseUrl);
        }

        // GET BY ID
        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<AccountResponse>($"{_baseUrl}/{id}");
        }

        // CREATE
        public async Task<AccountResponse?> CreateAsync(AccountRequest request)
        {
            var response = await _http.PostAsJsonAsync(_baseUrl, request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        // UPDATE
        public async Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request)
        {
            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        // BAN
        public async Task BanAsync(Guid id)
        {
            var response = await _http.PatchAsync($"{_baseUrl}/{id}/ban", null);
            response.EnsureSuccessStatusCode();
        }

        // UNBAN
        public async Task UnbanAsync(Guid id)
        {
            var response = await _http.PatchAsync($"{_baseUrl}/{id}/unban", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
