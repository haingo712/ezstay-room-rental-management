using ContractAPI.Model;

namespace ContractAPI.Repository.Interface;

public interface IContractRepository
{
    IQueryable<Contract> GetAllQueryable();
  //  Task<bool> HasContractAsync(Guid tenantId, Guid roomId);
    Task<IEnumerable<Contract>> GetAllByOwnerIdAsync(Guid ownerId);
 Task<IEnumerable<Contract>> GetAllByTenantIdAsync(Guid tenantId);
    Task<Contract?> GetByIdAsync(Guid id);
    Task<Contract> AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(Contract contract);
    Task<bool> ContractRoomIsActiveAsync(Guid roomId);
}