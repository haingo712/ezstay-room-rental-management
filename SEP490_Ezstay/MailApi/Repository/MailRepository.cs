using MailApi.Model;
using MailApi.Repository.Interface;
using MongoDB.Driver;

namespace MailApi.Repository
{
    public class MailRepository : IMailRepository
    {
        private readonly IMongoCollection<OtpVerification> _collection;

        public MailRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<OtpVerification>("OtpVerifications");
            
            // Tạo TTL index tự động xoá document sau khi ExpireAt < now()
            var indexKeys = Builders<OtpVerification>.IndexKeys.Ascending(x => x.ExpireAt);
            var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
            var indexModel = new CreateIndexModel<OtpVerification>(indexKeys, indexOptions);
            
            try
            {
                _collection.Indexes.CreateOne(indexModel);
            }
            catch (MongoCommandException)
            {
                // Index đã tồn tại, bỏ qua
            }
        }

        public async Task<OtpVerification?> GetByEmailAndCodeAsync(string email, string otp) =>
            await _collection.Find(x => x.Email == email && x.OtpCode == otp).FirstOrDefaultAsync();

        public async Task<OtpVerification?> GetByIdAsync(Guid id)
            => await _collection.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task<OtpVerification> AddAsync(OtpVerification otpVerification)
        {
            await _collection.InsertOneAsync(otpVerification);
            return otpVerification;
        }
        
        public async Task Update(OtpVerification otp) =>
            await _collection.ReplaceOneAsync(x => x.Id == otp.Id, otp);
        
        public async Task DeleteAsync(OtpVerification otpVerification)
            => await _collection.DeleteOneAsync(r => r.Id == otpVerification.Id);
    }
}