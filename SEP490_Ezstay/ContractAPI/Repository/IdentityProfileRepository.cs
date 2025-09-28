
using MongoDB.Driver;
using ContractAPI.Model;
using ContractAPI.Repository.Interface;

namespace ContractAPI.Repository;

public class IdentityProfileRepository : IIdentityProfileRepository
{
    private readonly IMongoCollection<IdentityProfile> _profiles;

    public IdentityProfileRepository(IMongoDatabase database)
    {
        _profiles = database.GetCollection<IdentityProfile>("IdentityProfiles");
    }

    public IQueryable<IdentityProfile> GetAllQueryable()
        => _profiles.AsQueryable();
    
    public async Task<IdentityProfile?> GetByIdAsync(Guid id)
        => await _profiles.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(IdentityProfile profile)
        => await _profiles.InsertOneAsync(profile);

    public async Task<IEnumerable<IdentityProfile>> AddMany(List<IdentityProfile> profile)
    {
      await _profiles.InsertManyAsync(profile);
     return profile;
    }
      

    public async Task UpdateAsync(IdentityProfile profile)
        => await _profiles.ReplaceOneAsync(x => x.Id == profile.Id, profile);

    public async Task DeleteAsync(IdentityProfile profile)
        => await _profiles.DeleteOneAsync(x => x.Id == profile.Id);
}
