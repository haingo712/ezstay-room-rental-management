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

     public async Task<List<RoomAmenityDto>> GetAmenityIdsByRoomId(Guid roomId)
     {
         var response = await _httpClient.GetFromJsonAsync<List<RoomAmenityDto>>(
             $"api/RoomAmenity/ByRoomId/{roomId}");

         return response ??  new List<RoomAmenityDto>();
     }
    //
    // public RoomAmenityClientService(IHttpClientFactory factory)
    // {
    //     _httpClient = factory.CreateClient("RoomAmenity");
    // }
    //
    // public async Task<List<Guid>> GetAmenityIdsByRoomId(Guid roomId)
    // {
    //     var response = await _httpClient.GetFromJsonAsync<List<Guid>>(
    //         $"api/RoomAmenity/ByRoomId/{roomId}");
    //
    //     return response ?? new List<Guid>();
    // }
}