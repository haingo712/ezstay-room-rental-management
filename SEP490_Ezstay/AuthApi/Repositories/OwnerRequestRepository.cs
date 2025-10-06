using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using MongoDB.Driver;

namespace AuthApi.Repositories
{
    public class OwnerRequestRepository : IOwnerRequestRepository
    {
        private readonly IMongoCollection<OwnerRegistrationRequest> _collection;

        public OwnerRequestRepository(MongoDbService dbService)
        {
            _collection = dbService.OwnerRequests; // property PascalCase
        }

        public async Task CreateAsync(OwnerRegistrationRequest request)
        {
            await _collection.InsertOneAsync(request);
        }

        public async Task<OwnerRegistrationRequest?> GetByIdAsync(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(OwnerRegistrationRequest request)
        {
            await _collection.ReplaceOneAsync(x => x.Id == request.Id, request);
        }   
    }
}
