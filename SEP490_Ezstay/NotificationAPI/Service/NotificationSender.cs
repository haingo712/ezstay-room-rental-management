using NotificationAPI.Service.Interfaces;

namespace NotificationAPI.Service
{
    public class NotificationSender : INotificationSender
    {
        private readonly HttpClient _httpClient;

        public NotificationSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendToAllAsync(string message)
        {
            var content = new StringContent($"\"{message}\"", System.Text.Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("http://localhost:7123/api/signalR", content); // URL SignalRHubAPI
        }
    }
}
