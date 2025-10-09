using ContractAPI.Model;
using MongoDB.Driver;
using ContractAPI.Enum;
using ContractAPI.Repository.Interface;
using Shared.Enums;

namespace ContractAPI.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly IMongoCollection<Contract> _contracts;

        public ContractRepository(IMongoDatabase database)
        {
            _contracts = database.GetCollection<Contract>("Contracts");
        }
        // public Task<bool> HasContractAsync(Guid tenantId, Guid roomId)
        // {
        //     return _contracts.Find(c => c.TenantId == tenantId && c.RoomId == roomId).AnyAsync();
        // }


        public IQueryable<Contract> GetAllQueryable() => _contracts.AsQueryable();
        
        public async Task<IEnumerable<Contract>> GetAllByOwnerIdAsync(Guid ownerId)
            => await _contracts.Find(t => t.OwnerId == ownerId).ToListAsync();
        
        // public async Task<IEnumerable<Contract>> GetAllByTenantIdAsync(Guid tenantId)
        //     => await _contracts.Find(t => t.TenantId == tenantId).ToListAsync();
        //
        public async Task<Contract?> GetByIdAsync(Guid id)
            => await _contracts.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task<Contract> AddAsync(Contract contract)
        {
           await _contracts.InsertOneAsync(contract);
            return contract;
        }
         
        
        public async Task UpdateAsync(Contract contract)
            => await _contracts.ReplaceOneAsync(t => t.Id == contract.Id, contract);
        
        public async Task DeleteAsync(Contract contract)
            => await _contracts.DeleteOneAsync(r => r.Id == contract.Id);
        
        public async Task<bool> ContractRoomIsActiveAsync(Guid roomId)
        {
            var filter = Builders<Contract>.Filter.Eq(t => t.RoomId, roomId) &
                          Builders<Contract>.Filter.Eq(t => t.ContractStatus, ContractStatus.Active);
            return await _contracts.Find(filter).AnyAsync();
        }
    }
}