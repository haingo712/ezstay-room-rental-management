using BoardingHouseAPI.Service.Interface;
using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient _httpClient;
        public RoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RoomResponse>?> GetRoomsByHouseIdAsync(Guid houseId)
        {
            var response = await _httpClient.GetAsync($"api/Rooms/house/{houseId}");
            if (response.IsSuccessStatusCode)
            {
                var rooms = await response.Content.ReadFromJsonAsync<List<RoomResponse>>();
                return rooms;
            }
            return null;
        }


    }
}
