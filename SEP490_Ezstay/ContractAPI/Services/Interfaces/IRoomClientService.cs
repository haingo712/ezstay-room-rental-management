using ContractAPI.DTO.Response;

namespace ContractAPI.Services.Interfaces;

public interface IRoomClientService
{
    Task<RoomResponse?> GetRoomByIdAsync(Guid roomId);
    Task<bool> UpdateRoomStatusAsync(Guid roomId, string status);
}