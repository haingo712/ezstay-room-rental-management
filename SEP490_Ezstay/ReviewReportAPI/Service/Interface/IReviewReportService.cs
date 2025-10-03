using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;

namespace ReviewReportAPI.Service.Interface;

public interface IReviewReportService
{
    Task<ApiResponse<ReviewReportResponse>> AddAsync(Guid reviewId, CreateReviewReportRequest request);
    Task<ApiResponse<ReviewReportResponse>> ApproveAsync(Guid reportId);
    Task<ApiResponse<ReviewReportResponse>> RejectAsync(Guid reportId, string reason);
    Task<IEnumerable<ReviewReportResponse>> GetAllAsync();
}
