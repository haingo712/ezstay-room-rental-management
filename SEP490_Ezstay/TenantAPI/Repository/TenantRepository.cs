using TenantAPI.Model;
using MongoDB.Driver;
using TenantAPI.Enum;
using TenantAPI.Repository.Interface;

namespace TenantAPI.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IMongoCollection<Tenant> _tenants;
        public TenantRepository(IMongoDatabase database)
        {
            _tenants = database.GetCollection<Tenant>("Tenants");
        }
        
        public IQueryable<Tenant> GetAllQueryable() => _tenants.AsQueryable();
        
        public async Task<IEnumerable<Tenant>> GetAllByOwnerIdAsync(Guid ownerId)
        {
            return await _tenants.Find(t => t.OwnerId == ownerId).ToListAsync();
        }
        
        public async Task<IEnumerable<Tenant>> GetAllByUserIdAsync(Guid userId)
        {
            return await _tenants.Find(t => t.OwnerId == userId).ToListAsync();
        }
        
        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _tenants.Find(t => t.Id == id).FirstOrDefaultAsync();
        }
        
        public async Task AddAsync(Tenant tenant)
        {
            await _tenants.InsertOneAsync(tenant);
        }
        
        public async Task UpdateAsync(Tenant tenant)
        {
            await _tenants.ReplaceOneAsync(t => t.Id == tenant.Id, tenant);
        }
        
        public async Task<bool> TenantRoomIsActiveAsync(Guid roomId)
        {
            var filter = Builders<Tenant>.Filter.Eq(t => t.RoomId, roomId) &
                         Builders<Tenant>.Filter.Eq(t => t.TenantStatus,TenantStatus.Active);
            return await _tenants.Find(filter).AnyAsync();
        }
    }
}