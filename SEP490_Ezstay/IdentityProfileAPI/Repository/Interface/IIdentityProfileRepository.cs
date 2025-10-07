

using IdentityProfileAPI.Model;

namespace IdentityProfileAPI.Repository.Interface;

public interface IIdentityProfileRepository
{
    IQueryable<IdentityProfile> GetAllQueryable();
    Task<IdentityProfile?> GetByIdAsync(Guid id);
    Task AddAsync(IdentityProfile profile);
    Task<IEnumerable<IdentityProfile>> AddMany(List<IdentityProfile> profile);
    Task UpdateAsync(IdentityProfile profile);
    Task DeleteAsync(IdentityProfile profile);
}