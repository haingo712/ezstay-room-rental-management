using UtilityRateAPI.Model;
using MongoDB.Driver;
using UtilityRateAPI.Enum;
using UtilityRateAPI.Repository.Interface;

namespace UtilityRateAPI.Repository;

public class UtilityRateRepository:IUtilityRateRepository
{
    private readonly IMongoCollection<UtilityRate> _utilityRates;
    public UtilityRateRepository(IMongoDatabase database)
    {
        _utilityRates = database.GetCollection<UtilityRate>("UtilityRates");
    }
    public IQueryable<UtilityRate> GetAllOdata()=> _utilityRates.AsQueryable();
    public async Task<IEnumerable<UtilityRate>> GetAll()
    {
        return await _utilityRates.Find(_ => true).ToListAsync();
    }
    public async Task<IEnumerable<UtilityRate>> GetAllByOwnerId(Guid ownerId)
    {
        return await _utilityRates.Find(a=> a.OwnerId == ownerId).ToListAsync();
    }
    public async Task<UtilityRate?> GetByIdAsync(Guid id)
    {
      return await _utilityRates.Find(a => a.Id == id).FirstOrDefaultAsync();
    }
   
    public async Task<List<UtilityRate>> GetAllByOwnerAndTypeAsync(Guid ownerId, UtilityType type)
    {
        return await _utilityRates
            .Find(x => x.OwnerId == ownerId && x.Type == type)
            .ToListAsync();
    }

    public async Task<UtilityRate> GetByOwnerTypeAndTierAsync(Guid ownerId, UtilityType type, int tier)
    {
        return await _utilityRates
            .Find(x => x.OwnerId == ownerId && x.Type == type && x.Tier == tier)
            .FirstOrDefaultAsync();
    }
    
    public async Task AddAsync(UtilityRate utilityRate)
    {
        await _utilityRates.InsertOneAsync(utilityRate);
    }
    public async Task UpdateAsync(UtilityRate utilityRate)
    {
        await _utilityRates.ReplaceOneAsync(a => a.Id == utilityRate.Id, utilityRate);
    }
    public async Task DeleteAsync(UtilityRate utilityRate)
    {
        await _utilityRates.DeleteOneAsync(a => a.Id == utilityRate.Id);
    }

}