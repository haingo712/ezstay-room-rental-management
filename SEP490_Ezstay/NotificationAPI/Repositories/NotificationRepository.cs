using AuthApi.Enums;
using MongoDB.Driver;
using NotificationAPI.Enums;
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

        public async Task<List<Notify>> GetByUserIdAsync(Guid userId)
        {
            return await _notifications.Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notify?> GetByIdAsync(Guid id)
        {
            return await _notifications.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Notify notify)
        {
            await _notifications.InsertOneAsync(notify);
        }

        public async Task UpdateAsync(Notify notify)
        {
            await _notifications.ReplaceOneAsync(x => x.Id == notify.Id, notify);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _notifications.DeleteOneAsync(x => x.Id == id);
        }

        public async Task CreateManyAsync(List<Notify> notifies)
        {
            await _notifications.InsertManyAsync(notifies);
        }

        public async Task<List<Notify>> GetByUserIdAsyncrole(Guid userId)
        {
            return await _notifications.Find(n => n.UserId == userId).ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var update = Builders<Notify>.Update.Set(n => n.IsRead, true);
            var result = await _notifications.UpdateOneAsync(n => n.Id == id, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsyncByRole(Guid id)
        {
            var result = await _notifications.DeleteOneAsync(n => n.Id == id);
            return result.DeletedCount > 0;
        }


        public async Task<List<Notify>> GetByUserIdsAsync(IEnumerable<Guid> userIds)
        {
            var filter = Builders<Notify>.Filter.In(n => n.UserId, userIds);
            return await _notifications.Find(filter).ToListAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<Notify> notifies)
        {
            var tasks = notifies.Select(async notify =>
            {
                var filter = Builders<Notify>.Filter.Eq(x => x.Id, notify.Id);
                await _notifications.ReplaceOneAsync(filter, notify);
            });
            await Task.WhenAll(tasks);
        }

        public async Task<List<Notify>> GetAllForRoleOrUserAsync(Guid userId, RoleEnum role)
        {
            if (role == RoleEnum.Admin)
            {
                // Admin thấy tất cả
                return await _notifications.Find(_ => true)
                    .SortByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }

            // Các role khác chỉ thấy:
            // - Thông báo chung cho role đó
            // - Thông báo riêng cho user đó
            var roleFilter = Builders<Notify>.Filter.Eq(x => x.TargetRole, role);
            var userFilter = Builders<Notify>.Filter.Eq(x => x.UserId, userId);

            // OR giữa 2 điều kiện trên
            var filter = Builders<Notify>.Filter.Or(roleFilter, userFilter);

            // Lấy danh sách thông báo theo filter
            var result = await _notifications.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            // Lọc kỹ lại lần nữa (đề phòng dữ liệu lỗi)
            result = result
                .Where(n =>
                    n.TargetRole == null ||
                    n.TargetRole == role ||
                    n.UserId == userId
                )
                .ToList();

            return result;
        }


        public async Task CreateAsync(Notify notify)
        {
            await _notifications.InsertOneAsync(notify);
        }
        public List<object> GetAllNotificationTypes()
        {
            var types = Enum.GetValues(typeof(NotificationType))
                            .Cast<NotificationType>()
                            .Where(t => t != NotificationType.OwnerRegister) // 👈 loại bỏ OwnerRegister
                            .Select(t => new
                            {
                                Id = (int)t,
                                Name = t.ToString()
                            })
                            .ToList<object>();

            return types;
        }


        public List<object> GetAllRoles()
        {
            var roles = Enum.GetValues(typeof(RoleEnum))
                            .Cast<RoleEnum>()
                            .Select(r => new
                            {
                                Id = (int)r,
                                Name = r.ToString()
                            })
                            .ToList<object>();

            return roles;
        }




    }
}
