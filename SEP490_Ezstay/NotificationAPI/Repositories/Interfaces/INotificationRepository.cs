using AuthApi.Enums;
using NotificationAPI.Model;

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
        Task<List<Notify>> GetAllForRoleOrUserAsync(Guid userId, RoleEnum role);



        }
}
