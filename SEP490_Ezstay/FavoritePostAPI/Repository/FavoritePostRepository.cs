using FavoritePostAPI.Models;
using FavoritePostAPI.Repository.Interface;
using MongoDB.Driver;

namespace FavoritePostAPI.Repository
{
    public class FavoritePostRepository : IFavoritePostRepository
    {
        private readonly IMongoCollection<FavoritePost> _collection;

        public FavoritePostRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<FavoritePost>("FavoritePosts");
        }

        public async Task<FavoritePost?> GetByAccountAndPostAsyn(Guid accountId, Guid postId)
        {
            return await _collection
                .Find(f => f.AccountId == accountId && f.PostId == postId)
                .FirstOrDefaultAsync();
        }

        public async Task<FavoritePost> CreateAsync(FavoritePost entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<FavoritePost>> GetByAccountAsync(Guid accountId)
        {
            return await _collection
                .Find(f => f.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<FavoritePost?> GetByIdAsync(Guid id)
        {
            return await _collection
                .Find(f => f.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsyn(Guid id)
        {
            var result = await _collection.DeleteOneAsync(f => f.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
