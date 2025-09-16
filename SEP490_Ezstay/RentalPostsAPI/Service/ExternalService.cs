using System.Net.Http.Json;
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

        public async Task<RoomDto?> GetRoomByIdAsync(Guid roomId)
        {
            var url = $"{_settings.RoomApiBaseUrl}/api/Rooms/{roomId}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<RoomDto>();
        }

        public async Task<BoardingHouseDTO?> GetBoardingHouseByIdAsync(Guid houseId)
        {
            var url = $"{_settings.BoardingHouseApiBaseUrl}/api/BoardingHouseAPI/{houseId}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<BoardingHouseDTO>();
        }

        public async Task<AccountDto?> GetAccountByIdAsync(Guid Id)
        {
            var url = $"{_settings.AuthApiBaseUrl}/api/AccountAPI/{Id}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<AccountDto>();
        }
    }
}
