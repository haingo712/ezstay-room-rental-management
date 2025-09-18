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
    
    // public async Task<bool> HasPostsForRoomAsync(Guid roomId)
    // {
    //     var response = await _httpClient.GetAsync($"odata/RentalPosts/?$filter=RoomId eq {roomId}");
    //     if (!response.IsSuccessStatusCode)
    //         return false;
    //     var json = await response.Content.ReadAsStringAsync();
    //     using var doc = JsonDocument.Parse(json);
    //
    //     if (!doc.RootElement.TryGetProperty("value", out var root))
    //         return false;
    //     return root.GetArrayLength() > 0;
    // }
    public async Task<bool> HasPostsForRoomAsync(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"odata/RentalPosts?$filter=RoomId eq {roomId}");
        if (!response.IsSuccessStatusCode) return false;
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0;
    }
    
}