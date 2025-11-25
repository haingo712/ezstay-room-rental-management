using RoomAPI.Service.Interface;
using Shared.DTOs.Amenities.Responses;

namespace RoomAPI.Service;

public class AmenityService : IAmenityService
{
    private readonly HttpClient _httpClient;

    public AmenityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<AmenityResponse?> GetAmenityById(Guid amenityId)
    {
        return await _httpClient.GetFromJsonAsync<AmenityResponse>($"/api/Amenity/{amenityId}");
    }
}