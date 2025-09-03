using TenantAPI.Model;

namespace TenantAPI.Repository.Interface;

public interface ITenantRepository
{
    IQueryable<Tenant> GetAllOdata();
    Task<IEnumerable<Tenant>> GetAll();
    Task<IEnumerable<Tenant>> GetAllById(Guid staffId);
    Task<Tenant?> GetByIdAsync(Guid id);
    Task AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task<bool> TenantRoomIsActiveAsync(Guid roomId);
}