using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using MongoDB.Driver;

namespace AuthApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountRepository(MongoDbService dbService)
        {
            _accounts = dbService.Account;
        }

        public async Task<Account?> GetByEmailAsync(string email) =>
            await _accounts.Find(a => a.Email == email).FirstOrDefaultAsync();

        public async Task<Account?> GetByPhoneAsync(string phone) =>
            await _accounts.Find(a => a.Phone == phone).FirstOrDefaultAsync();

        public async Task CreateAsync(Account account) =>
            await _accounts.InsertOneAsync(account);
        public async Task MarkAsVerified(string email)
        {
            var update = Builders<Account>.Update.Set(a => a.IsVerified, true);
            await _accounts.UpdateOneAsync(a => a.Email == email, update);
        }

    }
}
