using System.Text.Json;
using RoomAPI.DTO.Response;
using RoomAPI.Service.Interface;

namespace RoomAPI.Service;

public class RentalPostClientService: IRentalPostClientService
{
    private readonly HttpClient _httpClient;

    public RentalPostClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> HasPostsForRoomAsync(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"odata/RentalPosts?$filter=RoomId eq {roomId}");
        if (!response.IsSuccessStatusCode) return false;
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0;
    }
    
}