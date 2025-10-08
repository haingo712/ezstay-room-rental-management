using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;

namespace RoomAPI.APIs.Interfaces;

public interface IRoomAmenityAPI
{
    Task<ApiResponse<List<RoomAmenityResponse>>> AddRoomAmenitiesAsync(Guid roomId, List<CreateRoomAmenity> request);

}