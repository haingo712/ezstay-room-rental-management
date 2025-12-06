using ContractAPI.Model;

namespace ContractAPI.Repository.Interface;

public interface IContractRepository
{
  //  Task<bool> HasContractAsync(Guid tenantId, Guid roomId);
    IQueryable<Contract> GetAllByOwnerId(Guid ownerId);
    IQueryable<Contract> GetAllByTenantId(Guid userId);
    Task<Contract?> GetById(Guid id);
    Task<Contract> Add(Contract contract);
    Task Update(Contract contract);
    Task Delete(Contract contract);
    Task<bool> ExistsByRoomId(Guid roomId);
    
    Task<RentalRequest> Add(RentalRequest request);
    IQueryable<RentalRequest> GetAllRentalByOwnerId(Guid ownerId);
    IQueryable<RentalRequest> GetAllRentalByUserId(Guid userId);
    Task<RentalRequest> GetRentalRequestById(Guid id);
    
}