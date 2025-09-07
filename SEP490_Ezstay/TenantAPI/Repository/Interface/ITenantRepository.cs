using TenantAPI.Model;

namespace TenantAPI.Repository.Interface;

public interface ITenantRepository
{
    IQueryable<Tenant> GetAllQueryable();
    Task<IEnumerable<Tenant>> GetAllByOwnerIdAsync(Guid ownerId);
    Task<IEnumerable<Tenant>> GetAllByUserIdAsync(Guid userId);
    Task<Tenant?> GetByIdAsync(Guid id);
    Task AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task<bool> TenantRoomIsActiveAsync(Guid roomId);
}