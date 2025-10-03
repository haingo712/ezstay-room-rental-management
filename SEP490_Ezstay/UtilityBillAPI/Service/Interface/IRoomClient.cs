using UtilityBillAPI.DTO.Request;

namespace UtilityBillAPI.Service.Interface
{
    public interface IRoomClient
    {
        Task<RoomDTO?> GetRoomAsync(Guid roomId);
    }
}
