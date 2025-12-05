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
        private readonly IMongoCollection<RentalRequest> _contractRequests;

        public ContractRepository(IMongoDatabase database)
        {
            _contracts = database.GetCollection<Contract>("Contracts");
            _contractRequests = database.GetCollection<RentalRequest>("RentalRequests");
        }
        public IQueryable<Contract> GetAllByOwnerId(Guid ownerId)
            => _contracts.AsQueryable().Where(c => c.ProfilesInContract.Any(p => p.UserId == ownerId));
        
        public IQueryable<Contract> GetAllByTenantId(Guid userId)
            => _contracts.AsQueryable().Where(c => c.ProfilesInContract.Any(p => p.UserId == userId));
        
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
        
        public async Task<RentalRequest> Add(RentalRequest request)
        {
            await _contractRequests.InsertOneAsync(request);
            return request;
        }
        public IQueryable<RentalRequest> GetAllRentalByOwnerId(Guid ownerId)
            => _contractRequests.AsQueryable().Where(p => p.OwnerId  == ownerId);
        
        public IQueryable<RentalRequest> GetAllRentalByUserId(Guid userId)
            => _contractRequests.AsQueryable().Where(p => p.UserId  == userId);


        public async Task<RentalRequest> GetRentalRequestById(Guid id)
        {
            return await _contractRequests
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

      
    }
}