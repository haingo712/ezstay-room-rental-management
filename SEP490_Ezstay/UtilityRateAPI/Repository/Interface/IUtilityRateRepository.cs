using UtilityRateAPI.Enum;
using UtilityRateAPI.Model;

namespace UtilityRateAPI.Repository.Interface;

public interface IUtilityRateRepository
{
    IQueryable<UtilityRate> GetAllOdata();
    Task<IEnumerable<UtilityRate>> GetAll();
    Task<IEnumerable<UtilityRate>> GetAllByOwnerId(Guid ownerId);
    Task<UtilityRate?> GetByIdAsync(Guid id);
    Task AddAsync(UtilityRate utilityRate);
    Task UpdateAsync(UtilityRate utilityRate);
    Task DeleteAsync(UtilityRate utilityRate);
    Task<List<UtilityRate>> GetAllByOwnerAndTypeAsync (Guid ownerId, UtilityType type);
    Task<UtilityRate> GetByOwnerTypeAndTierAsync(Guid ownerId, UtilityType type, int tier);
}