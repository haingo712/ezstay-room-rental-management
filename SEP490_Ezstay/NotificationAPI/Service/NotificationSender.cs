using AuthApi.Enums;
using NotificationAPI.Service.Interfaces;
using System.Text.Json;
using System.Text;

namespace NotificationAPI.Service
{
    public class NotificationSender : INotificationSender
    {
        private readonly HttpClient _httpClient;

        public NotificationSender(HttpClient httpClient, IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            _httpClient = factory.CreateClient("Gateway");
        }

        public async Task SendToAllAsync(string message)
        {
            var content = new StringContent($"\"{message}\"", System.Text.Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("http://localhost:7123/api/signalR", content); // URL SignalRHubAPI
        }

        public async Task<List<object>> GetByRoleAsync(RoleEnum role)
        {
            var json = JsonSerializer.Serialize(role);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Gọi qua Gateway → AuthAPI (qua route /api/accounts/GetByRole)
            var response = await _httpClient.PostAsync("api/Accounts/GetByRole", content);

            response.EnsureSuccessStatusCode();

            // Deserialize về danh sách tài khoản
            var body = await response.Content.ReadAsStringAsync();
            var accounts = JsonSerializer.Deserialize<List<object>>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return accounts ?? new List<object>();
        }
    }
}
