namespace BoardingHouseAPI.Service.Interface;

public interface IContractService
{
    Task<bool> ContractExistsByRoomId(Guid roomId);
}