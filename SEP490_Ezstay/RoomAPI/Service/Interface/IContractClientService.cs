namespace RoomAPI.Service.Interface;

public interface IContractClientService
{
    Task<bool> HasContractByRoomId(Guid roomId);
}