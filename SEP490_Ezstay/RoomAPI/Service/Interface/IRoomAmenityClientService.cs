using RoomAPI.DTO.Response;

namespace RoomAPI.Service.Interface;

public interface IRoomAmenityClientService
{
    Task<List<RoomAmenityDto>> GetAmenityIdsByRoomId(Guid roomId);

}