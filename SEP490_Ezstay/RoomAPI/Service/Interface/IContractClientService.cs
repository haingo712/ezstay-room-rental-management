namespace RoomAPI.Service.Interface;

public interface IContractClientService
{
    Task<bool> ContractExistsByRoomId(Guid roomId);
}