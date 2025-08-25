using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using MongoDB.Driver;

namespace AuthApi.Repositories
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly IMongoCollection<EmailVerification> _collection;

        public EmailVerificationRepository(MongoDbService db)
        {
            _collection = db.EmailVerifications;
        }

        public async Task CreateAsync(EmailVerification verification)
        {
            await _collection.InsertOneAsync(verification);
        }

        public async Task<EmailVerification?> GetByEmailAsync(string email)
        {
            return await _collection.Find(x => x.Email == email && !x.IsVerified).FirstOrDefaultAsync();
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var filter = Builders<EmailVerification>.Filter.Where(x =>
                x.Email == email &&
                x.OtpCode == otp &&
                x.ExpiredAt > DateTime.UtcNow &&
                !x.IsVerified);

            var update = Builders<EmailVerification>.Update
                .Set(x => x.IsVerified, true);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
