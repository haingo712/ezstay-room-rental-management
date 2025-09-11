using AccountAPI.Data;
using AccountAPI.Repositories.Interfaces;
using MongoDB.Driver;

namespace AccountAPI.Repositories
{
    public class UserRepository : IUserRepository   
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("Users");
        }

        public async Task CreateUserAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task<User?> GetByUserIdAsync(Guid userId)
        {
            return await _collection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, user.UserId);
            await _collection.ReplaceOneAsync(filter, user);
        }
    
    }
}
