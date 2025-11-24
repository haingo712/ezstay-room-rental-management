using MongoDB.Bson.IO;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Shared.DTOs.Rooms.Responses;
using Shared.Enums;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ContractAPI.Services;

public class RoomService : IRoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
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
    public async Task<bool> UpdateRoomStatusAsync(Guid roomId, RoomStatus status)
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
