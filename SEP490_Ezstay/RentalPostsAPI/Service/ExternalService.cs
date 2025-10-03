using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.Service.Interface;

namespace RentalPostsAPI.Service
{
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalServiceSettings _settings;

        public ExternalService(HttpClient httpClient, IOptions<ExternalServiceSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        private async Task<T?> GetFromApiAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return default;

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content)) return default;

                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // TODO: log lỗi ra file / console
                Console.WriteLine($"[ExternalService] Error calling {url}: {ex.Message}");
                return default;
            }
        }

        public Task<RoomDto?> GetRoomByIdAsync(Guid roomId)
        {
            var url = $"{_settings.RoomApiBaseUrl}api/Rooms/{roomId}";
            return GetFromApiAsync<RoomDto>(url);
        }

        public Task<BoardingHouseDTO?> GetBoardingHouseByIdAsync(Guid houseId)
        {
            var url = $"{_settings.BoardingHouseApiBaseUrl}api/BoardingHouses/{houseId}";
            return GetFromApiAsync<BoardingHouseDTO>(url);
        }

        public Task<AccountDto?> GetAccountByIdAsync(Guid id)
        {
            var url = $"{_settings.AccountApiBaseUrl}api/Accounts/{id}";
            return GetFromApiAsync<AccountDto>(url);
        }
    }
}
