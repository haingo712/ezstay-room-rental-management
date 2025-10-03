using AccountAPI.Data;
using AccountAPI.DTO.Request;
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

        public async Task UpdateAsync(User user)
        {
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<User> GetPhone(string phone)
        {
            return await _collection.Find(u => u.Phone == phone).FirstOrDefaultAsync();
        }

    }
}
