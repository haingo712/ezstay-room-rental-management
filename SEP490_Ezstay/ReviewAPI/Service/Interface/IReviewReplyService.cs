using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response;
using ReviewAPI.DTO.Response.ReviewReply;

namespace ReviewAPI.Service.Interface;

public interface IReviewReplyService
{
    IQueryable<ReviewReplyResponse> GetAllQueryable();
    Task<ReviewReplyResponse?> GetByIdAsync(Guid id);
    Task<ApiResponse<ReviewReplyResponse>> AddAsync(Guid reviewId, CreateReviewReplyRequest reviewReply);
    Task<ReviewReplyResponse> GetReplyByReviewIdAsync(Guid reviewId);
    Task<ApiResponse<bool>> UpdateReplyAsync(Guid replyId, UpdateReviewReplyRequest reviewReply);
    Task DeleteReplyAsync(Guid replyId);
}