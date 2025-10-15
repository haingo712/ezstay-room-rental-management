using RoomAPI.APIs.Interfaces;
using RoomAPI.DTO.Request;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.APIs;

public class RoomAmenityAPI:IRoomAmenityAPI
{
    private readonly HttpClient _httpClient;

    public RoomAmenityAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<RoomAmenityResponse>>> AddRoomAmenitiesAsync(Guid roomId, List<CreateRoomAmenity> request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/RoomAmenity/{roomId}/amenity", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<List<RoomAmenityResponse>>.Fail("Không thể thêm tiện ích cho phòng.");
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<List<RoomAmenityResponse>>>();
    }

    public async Task<List<RoomAmenityResponse>> GetAmenityIdsByRoomId(Guid roomId)
    {
        return await _httpClient.GetFromJsonAsync<List<RoomAmenityResponse>>($"api/RoomAmenity/byRoomId/{roomId}");
    }
}