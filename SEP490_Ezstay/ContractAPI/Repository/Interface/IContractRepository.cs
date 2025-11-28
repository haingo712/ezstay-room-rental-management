using ContractAPI.Model;

namespace ContractAPI.Repository.Interface;

public interface IContractRepository
{
  //  Task<bool> HasContractAsync(Guid tenantId, Guid roomId);
    IQueryable<Contract> GetAllByOwnerId(Guid ownerId);
 //Task<IEnumerable<Contract>> GetAllByTenantId(Guid tenantId);
    Task<Contract?> GetById(Guid id);
    Task<Contract> Add(Contract contract);
    Task Update(Contract contract);
    Task Delete(Contract contract);
    Task<bool> ExistsByRoomId(Guid roomId);
}