
using MongoDB.Driver;
using TenantAPI.Model;
using TenantAPI.Repository.Interface;

namespace TenantAPI.Repository;

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

    public async Task UpdateAsync(IdentityProfile profile)
        => await _profiles.ReplaceOneAsync(x => x.Id == profile.Id, profile);

    public async Task DeleteAsync(IdentityProfile profile)
        => await _profiles.DeleteOneAsync(x => x.Id == profile.Id);
}
