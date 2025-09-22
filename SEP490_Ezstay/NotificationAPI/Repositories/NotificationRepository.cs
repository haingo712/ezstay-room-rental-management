using MongoDB.Driver;
using NotificationAPI.Model;
using NotificationAPI.Repositories.Interfaces;

namespace NotificationAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<Notify> _notifications;

        public NotificationRepository(IMongoDatabase settings)
        {
           
            _notifications = settings.GetCollection<Notify>("Notify");
        }

        public async Task<IEnumerable<Notify>> GetByUserIdAsync(Guid userId)
        {
            var filter = Builders<Notify>.Filter.Eq(n => n.UserId, userId);
            var sort = Builders<Notify>.Sort.Descending(n => n.CreatedAt);
            var result = await _notifications.Find(filter).Sort(sort).ToListAsync();
            return result;
        }

        public async Task<Notify?> GetByIdAsync(Guid id)
        {
            var filter = Builders<Notify>.Filter.Eq(n => n.Id, id);
            return await _notifications.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Notify notify)
        {
            await _notifications.InsertOneAsync(notify);
        }

        public async Task UpdateAsync(Notify notify)
        {
            var filter = Builders<Notify>.Filter.Eq(n => n.Id, notify.Id);
            await _notifications.ReplaceOneAsync(filter, notify);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<Notify>.Filter.Eq(n => n.Id, id);
            await _notifications.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Notify>> GetAllAsync()
        {
            var sort = Builders<Notify>.Sort.Descending(n => n.CreatedAt);
            return await _notifications.Find(_ => true).Sort(sort).ToListAsync();
        }

    }
}
