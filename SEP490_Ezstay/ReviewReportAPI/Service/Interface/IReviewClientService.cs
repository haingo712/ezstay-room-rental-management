using ReviewReportAPI.DTO.Response;

namespace ReviewReportAPI.Service.Interface;

public interface IReviewClientService
{
    Task<ReviewResponse?> GetReviewByIdAsync(Guid reviewId);
}