using RentalPostsAPI.Models;

namespace RentalPostsAPI.Repository.Interface
{
    public interface IRentalPostRepository
    {
        Task<IEnumerable<RentalPosts>> GetAllByOwnerIdAsync(Guid ownerId);
        Task<RentalPosts> CreateAsync(RentalPosts post);
        Task<RentalPosts?> GetByIdAsync(Guid id);
        Task<IEnumerable<RentalPosts>> GetAllAsync();
        Task<RentalPosts?> UpdateAsync(RentalPosts post);
        Task<bool> DeleteAsync(Guid id, Guid deletedBy);
    }
}
