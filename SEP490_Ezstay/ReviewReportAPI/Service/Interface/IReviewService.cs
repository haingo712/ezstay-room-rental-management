using Shared.DTOs.Reviews.Responses;

namespace ReviewReportAPI.Service.Interface;

public interface IReviewService
{
   // Task<ReviewResponse?> GetReviewById(Guid reviewId);
    Task<bool> HideReview(Guid reviewId, bool hide);
}