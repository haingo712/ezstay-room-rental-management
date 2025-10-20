using ReviewAPI.Model;

namespace ReviewAPI.Repository.Interface;

public interface IReviewRepository
{
    IQueryable<Review> GetAllAsQueryable();
    Task<IEnumerable<Review>> GetAll();
    Task<Review?> GetByContractIdAsync(Guid contractId);
    // Task<IEnumerable<Review>> GetAllByPostId(Guid postId);
    Task<Review?> GetByIdAsync(Guid id);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
     Task DeleteAsync(Review review);
}