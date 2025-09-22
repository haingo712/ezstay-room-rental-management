

using TenantAPI.Model;

namespace TenantAPI.Repository.Interface;

public interface IIdentityProfileRepository
{
    IQueryable<IdentityProfile> GetAllQueryable();
    Task<IdentityProfile?> GetByIdAsync(Guid id);
    Task AddAsync(IdentityProfile profile);
    Task UpdateAsync(IdentityProfile profile);
    Task DeleteAsync(IdentityProfile profile);
}