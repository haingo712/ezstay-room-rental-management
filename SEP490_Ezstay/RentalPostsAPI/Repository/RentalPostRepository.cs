using MongoDB.Driver;
using RentalPostsAPI.Data;
using RentalPostsAPI.Models;
using RentalPostsAPI.Repository.Interface;

namespace RentalPostsAPI.Repository
{
    public class RentalPostRepository : IRentalPostRepository
    {
        private readonly IMongoCollection<RentalPosts> _collection;

        public RentalPostRepository(MongoDbService dbService)
        {
            _collection = dbService.RentalPosts;
        }
        public IQueryable<RentalPosts> GetAllAsQueryable()=> _collection.AsQueryable();

        public async Task<RentalPosts> CreateAsync(RentalPosts post)
        {
            await _collection.InsertOneAsync(post);
            return post;
        }

        public async Task<IEnumerable<RentalPosts>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<RentalPosts?> GetByIdAsync(Guid id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RentalPosts?> UpdateAsync(RentalPosts post)
        {
            var updateResult = await _collection.ReplaceOneAsync(p => p.Id == post.Id, post);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                return post;
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid deletedBy)
        {
            var update = Builders<RentalPosts>.Update
                .Set(p => p.IsActive, false)
                .Set(p => p.DeletedAt, DateTime.UtcNow)
                .Set(p => p.DeletedBy, deletedBy);

            var result = await _collection.UpdateOneAsync(p => p.Id == id, update);
            return result.ModifiedCount > 0;
        }
        
        public async Task<IEnumerable<RentalPosts>> GetByRoomIdAsync(Guid roomId)
        {
            return await _collection.Find(x => x.RoomId == roomId).ToListAsync();
        }
    }
}
