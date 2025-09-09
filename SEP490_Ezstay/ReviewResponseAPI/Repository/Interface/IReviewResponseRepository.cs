using ReviewResponseAPI.Model;

namespace ReviewResponseAPI.Repository.Interface;

public interface IReviewResponseRepository
{
    IQueryable<ReviewResponse> GetAllAsQueryable();
    Task<IEnumerable<ReviewResponse>> GetAll();
    Task<IEnumerable<ReviewResponse>> GetAllByOwnerId(Guid ownerId);
    Task<ReviewResponse?> GetByIdAsync(Guid id);
    Task AddAsync(ReviewResponse reviewResponse);
    Task UpdateAsync(ReviewResponse reviewResponse);
    Task DeleteAsync(ReviewResponse reviewResponse);
    Task<IEnumerable<ReviewResponse>> GetAllByReviewId(Guid reviewId);
}