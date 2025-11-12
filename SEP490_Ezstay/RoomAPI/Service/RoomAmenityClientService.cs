using RoomAPI.DTO.Request;
using RoomAPI.Service.Interface;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.Service;

public class RoomAmenityClientService: IRoomAmenityClientService
{
     private readonly HttpClient _httpClient;
     
     public RoomAmenityClientService(HttpClient httpClient)
     {
         _httpClient = httpClient;
     }

     public async Task<List<RoomAmenityResponse>> GetAmenityIdsByRoomId(Guid roomId)
     {
         var response = await _httpClient.GetFromJsonAsync<List<RoomAmenityResponse>>(
             $"api/RoomAmenity/ByRoomId/{roomId}");

         return response ??  new List<RoomAmenityResponse>();
     }

     public async Task<bool> DeleteAmenityByRoomId(Guid roomId)
     {
         var response = await _httpClient.DeleteAsync($"api/RoomAmenity/byRoomId/{roomId}");
         return response.IsSuccessStatusCode;
     }

     public async Task AddAsync(Guid roomId, List<CreateRoomAmenity> request)
     {
         var response = await _httpClient.PostAsJsonAsync($"api/RoomAmenity/{roomId}/Amenity", request);
         response.EnsureSuccessStatusCode();
     }

   
}