using MongoDB.Driver;
using NotificationAPI.Model;
using Shared.Enums;

namespace NotificationAPI.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notify>> GetByUserIdAsync(Guid userId);
        Task<Notify?> GetByIdAsync(Guid id);
        Task AddAsync(Notify notify);
        Task UpdateAsync(Notify notify);
        Task DeleteAsync(Guid id);

        Task CreateManyAsync(List<Notify> notifies);
        Task<List<Notify>> GetByUserIdAsyncrole(Guid userId);
        Task<bool> MarkAsReadAsync(Guid id);
        Task<bool> DeleteAsyncByRole(Guid id);

        Task<List<Notify>> GetByUserIdsAsync(IEnumerable<Guid> userIds);
        Task UpdateManyAsync(IEnumerable<Notify> notifies);
        Task<List<Notify>> GetAllForUserAsync(Guid userId);
        Task CreateAsync(Notify notify);
        List<object> GetAllNotificationTypes();
        List<object> GetAllRoles();

        Task<List<Notify>> GetDueNotifiesAsync();
        Task MarkAsSentAsync(Guid id);

        Task<UpdateResult> UpdateManyAsync(FilterDefinition<Notify> filter, UpdateDefinition<Notify> update);



    }
}
