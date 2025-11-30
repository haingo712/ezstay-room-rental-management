using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IRoomService
    {
        Task<List<RoomResponse>> GetRoomsByHouseIdAsync(Guid houseId);
        Task<bool> DeleteRoomOnlyAsync(Guid roomId);
    }
}
