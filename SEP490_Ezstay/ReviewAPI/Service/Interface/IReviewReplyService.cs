using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response;
using ReviewAPI.DTO.Response.ReviewReply;
using Shared.DTOs;

namespace ReviewAPI.Service.Interface;

public interface IReviewReplyService
{
    IQueryable<ReviewReplyResponse> GetAll();
    Task<ReviewReplyResponse?> GetByIdAsync(Guid id);
    Task<ApiResponse<ReviewReplyResponse>> Add(Guid reviewId, Guid ownerId, CreateReviewReplyRequest request);
    Task<ReviewReplyResponse> GetReplyByReviewIdAsync(Guid reviewId);
    Task<ApiResponse<bool>> UpdateReplyAsync(Guid id, UpdateReviewReplyRequest request);
    Task Delete(Guid reviewReplyId);
}