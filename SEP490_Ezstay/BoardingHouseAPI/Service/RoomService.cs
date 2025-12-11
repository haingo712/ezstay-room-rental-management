using BoardingHouseAPI.Service.Interface;
using BoardingHouseAPI.DTO.Response;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardingHouseAPI.Service
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public RoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task<List<RoomResponse>?> GetRoomsByHouseIdAsync(Guid houseId)
        {
            var response = await _httpClient.GetAsync($"api/Rooms/house/{houseId}");
            if (response.IsSuccessStatusCode)
            {
                var rooms = await response.Content.ReadFromJsonAsync<List<RoomResponse>>(_jsonOptions);
                return rooms;
            }
            return null;
        }
        public async Task<bool> DeleteRoomOnlyAsync(Guid roomId)
        {
            var response = await _httpClient.DeleteAsync($"api/Rooms/{roomId}");
            return response.IsSuccessStatusCode;
        }

    }
}
