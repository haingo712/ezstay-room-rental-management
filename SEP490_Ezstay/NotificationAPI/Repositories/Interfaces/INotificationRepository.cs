using NotificationAPI.Model;

namespace NotificationAPI.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notify>> GetByUserIdAsync(Guid userId);
        Task<Notify?> GetByIdAsync(Guid id);
        Task<IEnumerable<Notify>> GetAllAsync(); // ➕ thêm mới
        Task AddAsync(Notify notify);
        Task UpdateAsync(Notify notify);
        Task DeleteAsync(Guid id);
    }
}
