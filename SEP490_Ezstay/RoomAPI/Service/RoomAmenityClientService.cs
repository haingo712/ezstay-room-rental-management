using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;
using RoomAPI.Service.Interface;

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
     public async Task AddAsync(Guid roomId, List<CreateRoomAmenity> request)
     {
         var response = await _httpClient.PostAsJsonAsync($"api/RoomAmenity/{roomId}/Amenity", request);
         response.EnsureSuccessStatusCode();
     }

   
}