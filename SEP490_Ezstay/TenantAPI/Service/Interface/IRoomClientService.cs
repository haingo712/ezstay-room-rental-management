using TenantAPI.DTO.Response;

namespace TenantAPI.Service.Interface;

public interface IRoomClientService
{
    Task<RoomResponse?> GetRoomByIdAsync(Guid roomId);
    Task<bool> UpdateRoomStatusAsync(Guid roomId, string status);
}