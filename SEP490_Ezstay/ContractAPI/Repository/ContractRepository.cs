using ContractAPI.Model;
using MongoDB.Driver;
using ContractAPI.Enum;
using ContractAPI.Repository.Interface;
using MongoDB.Driver.Linq;
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
        public IQueryable<Contract> GetAllByOwnerId(Guid ownerId)
            => _contracts.AsQueryable().Where(c => c.ProfilesInContract.Any(p => p.UserId == ownerId));
        
        public IQueryable<Contract> GetAllByTenantId(Guid tenantId)
            => _contracts.AsQueryable().Where(c => c.ProfilesInContract.Any(p => p.UserId == tenantId));
        
        public async Task<Contract?> GetById(Guid id)
            => await _contracts.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task<Contract> Add(Contract contract)
        {
            await _contracts.InsertOneAsync(contract);
            return contract;
        }
        
        public async Task Update(Contract contract)
            => await _contracts.ReplaceOneAsync(t => t.Id == contract.Id, contract);
        
        public async Task Delete(Contract contract)
            => await _contracts.DeleteOneAsync(r => r.Id == contract.Id);
        
        public async Task<bool> ContractRoomIsActive(Guid roomId)
        {
            var filter = Builders<Contract>.Filter.Eq(t => t.RoomId, roomId) &
                          Builders<Contract>.Filter.Eq(t => t.ContractStatus, ContractStatus.Active);
            return await _contracts.Find(filter).AnyAsync();
        }
        public async Task<bool> ExistsByRoomId(Guid roomId)
        {
            return await _contracts.Find(c => c.RoomId == roomId).AnyAsync();
        }
    }
}