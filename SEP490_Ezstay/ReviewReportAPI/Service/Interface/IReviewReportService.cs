using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Enum;
using Shared.DTOs;

namespace ReviewReportAPI.Service.Interface;

public interface IReviewReportService
{
    IQueryable<ReviewReportResponse> GetAll();
    Task<ReviewReportResponse> GetById(Guid id);
    Task<ApiResponse<ReviewReportResponse>> Add(Guid reviewId, CreateReviewReportRequest request);
    Task<ApiResponse<bool>> Update(Guid id, UpdateReviewReportRequest request);
    Task<ApiResponse<bool>> SetStatus(Guid id, UpdateReportStatusRequest request);
    // Task<ApiResponse<ReviewReportResponse>> Approve(Guid reportId);
    // Task<ApiResponse<ReviewReportResponse>> Reject(Guid reportId, string reason);
 
}
