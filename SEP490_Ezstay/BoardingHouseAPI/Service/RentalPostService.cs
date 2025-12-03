using System.Text.Json;
using BoardingHouseAPI.Service.Interface;

namespace BoardingHouseAPI.Service;

public class RentalPostService: IRentalPostService
{
    private readonly HttpClient _httpClient;

    public RentalPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> RentalPostExistsByRoomId(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"odata/RentalPosts?$filter=RoomId eq {roomId}");
        if (!response.IsSuccessStatusCode) return false;
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0;
    }
    
}