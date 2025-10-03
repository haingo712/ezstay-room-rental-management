using MongoDB.Bson.IO;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ContractAPI.Services;

public class RoomClientService : IRoomClientService
{
    private readonly HttpClient _httpClient;

    public RoomClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RoomResponse?> GetRoomByIdAsync(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"api/rooms/{roomId}");
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RoomResponse>(content);
    }
    public async Task<bool> UpdateRoomStatusAsync(Guid roomId, string status)
    {
        // var payload = new { RoomStatus = status };
        var response = await _httpClient.PutAsync($"api/Rooms/{roomId}/RoomStatus/{status}",null);
        // var content = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(response.StatusCode);
        // Console.WriteLine(content);
        // Console.WriteLine(_httpClient.BaseAddress + $"Rooms/{roomId}/RoomStatus/{status}");
        return response.IsSuccessStatusCode;
    }
}
