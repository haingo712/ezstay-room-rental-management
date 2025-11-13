using AuthApi.Data;

using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Enums;

namespace AuthApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AuthRepository(MongoDbService dbService)
        {
            _accounts = dbService.Account;
        }

        public async Task<Account?> GetByEmailAsync(string email) =>
            await _accounts.Find(a => a.Email == email).FirstOrDefaultAsync();

        public async Task<Account?> GetByPhoneAsync(string phone) =>
            await _accounts.Find(a => a.Phone == phone).FirstOrDefaultAsync();

        public async Task<Account?> GetByIdAsync(Guid id) =>
            await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task<List<Account>> GetAllAsync() =>
            await _accounts.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Account account) =>
            await _accounts.InsertOneAsync(account);

        public async Task<Account?> UpdateAsync(Account account)
        {
            var result = await _accounts.ReplaceOneAsync(a => a.Id == account.Id, account);
            return result.ModifiedCount > 0 ? account : null;
        }

        public async Task MarkAsVerified(string email)
        {
            var update = Builders<Account>.Update.Set(a => a.IsVerified, true);
            await _accounts.UpdateOneAsync(a => a.Email == email, update);
        }
        public async Task BanAccountAsync(Guid id, bool isBanned)
        {
            var update = Builders<Account>.Update.Set(a => a.IsBanned, isBanned);
            await _accounts.UpdateOneAsync(a => a.Id == id, update);
        }

        public async Task<List<Account>> GetByRoleAsync(RoleEnum role)
        {
            return await _accounts.Find(a => a.Role == role).ToListAsync();
        }

        public async Task AddAsync(Account account) => await CreateAsync(account);


    }
}
