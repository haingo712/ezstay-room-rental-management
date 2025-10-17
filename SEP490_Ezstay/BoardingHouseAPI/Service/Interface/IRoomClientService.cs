using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IRoomClientService
    {
        Task<List<RoomResponse>> GetRoomsByHouseIdAsync(Guid houseId);
    }
}
