using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Service
{
    public class RoomClient : IRoomClient
    {
        private readonly HttpClient _httpClient;
        public RoomClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<RoomDTO?> GetRoomAsync(Guid roomId)
        {           
            var requestUrl = $"api/Rooms/{roomId}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoomDTO>();                
            }
            return null;
        }
    }
}
