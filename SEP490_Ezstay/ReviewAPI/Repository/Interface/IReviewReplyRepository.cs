using ReviewAPI.Model;

namespace ReviewAPI.Repository.Interface;

public interface IReviewReplyRepository
{
    IQueryable<ReviewReply> GetAll();
    Task<ReviewReply?> GetById(Guid id);
    Task<ReviewReply?> GetByReviewId(Guid reviewId);
    Task Add(ReviewReply reviewReply);
    Task Update(ReviewReply reviewReply);
    Task Delete(Guid id);
}