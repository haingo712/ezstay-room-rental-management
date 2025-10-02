using ReviewAPI.Model;

namespace ReviewAPI.Repository.Interface;

public interface IReviewReplyRepository
{
    IQueryable<ReviewReply> GetAllQueryable();
    Task<ReviewReply?> GetByIdAsync(Guid id);
    Task<ReviewReply?> GetByReviewIdAsync(Guid reviewId);
    Task AddAsync(ReviewReply reply);
    Task UpdateAsync(ReviewReply reply);
    Task DeleteAsync(Guid replyId);
}