using RoomAPI.DTO.Response;
using RoomAPI.Service.Interface;

namespace RoomAPI.Service;

public class AmenityClientService : IAmenityClientService
{
    private readonly HttpClient _httpClient;

    public AmenityClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<AmenityDto?> GetAmenityById(Guid amenityId)
    {
        return await _httpClient.GetFromJsonAsync<AmenityDto>($"api/Amenity/{amenityId}");
    }
}