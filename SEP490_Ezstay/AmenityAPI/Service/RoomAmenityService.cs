using AmenityAPI.Service.Interface;

namespace AmenityAPI.Service;

public class RoomAmenityService : IRoomAmenityService
{
    private readonly HttpClient _client;

    public RoomAmenityService(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> RoomAmenityExistsByAmenityId(Guid amenityId)
    {
        var response = await _client.GetAsync($"api/RoomAmenity/check-by-amenity/{amenityId}");
        if (!response.IsSuccessStatusCode)
            return false;
        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }
}
