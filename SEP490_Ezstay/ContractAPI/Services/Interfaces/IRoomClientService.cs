using ContractAPI.DTO.Response;
using Shared.DTOs.Rooms.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;

public interface IRoomClientService
{
    Task<RoomResponse?> GetRoomByIdAsync(Guid roomId);
    Task<bool> UpdateRoomStatusAsync(Guid roomId, RoomStatus status);
}