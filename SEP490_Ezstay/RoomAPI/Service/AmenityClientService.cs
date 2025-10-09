using RoomAPI.Service.Interface;
using Shared.DTOs.Amenities.Responses;

namespace RoomAPI.Service;

public class AmenityClientService : IAmenityClientService
{
    private readonly HttpClient _httpClient;

    public AmenityClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<AmenityResponse?> GetAmenityById(Guid amenityId)
    {
        return await _httpClient.GetFromJsonAsync<AmenityResponse>($"api/Amenity/{amenityId}");
    }
}