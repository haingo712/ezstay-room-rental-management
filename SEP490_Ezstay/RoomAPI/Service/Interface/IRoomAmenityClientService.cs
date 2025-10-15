
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.Service.Interface;

public interface IRoomAmenityClientService
{
    Task<List<RoomAmenityResponse>> GetAmenityIdsByRoomId(Guid roomId);

}