namespace RoomAPI.Service.Interface;

public interface IContractService
{
    Task<bool> ContractExistsByRoomId(Guid roomId);
}