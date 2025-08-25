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
    Task<List<UtilityRate>> GetAllByTypeAsync(UtilityType type);
    //Task<UtilityRate?> GetPreviousTierAsync(Guid ownerId, UtilityType type, int currentTier);
    Task<decimal> GetMaxToByTypeAndOwnerAndTierAsync (Guid ownerId, UtilityType type, int tier);
    Task<int> GetMaxTierByTypeAndOwnerAsync(Guid ownerId, UtilityType type);
}