using AutoMapper;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Model;
using ReviewReportAPI.Repository.Interface;
using ReviewReportAPI.Service.Interface;

namespace ReviewReportAPI.Service;

public class ReviewReportService : IReviewReportService
{
    private readonly IReviewReportRepository _reportRepository;
   // private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    // public ReviewReportService(IReviewReportRepository reportRepository, IReviewRepository reviewRepository, IMapper mapper)
    // {
    //     _reportRepository = reportRepository;
    //     _reviewRepository = reviewRepository;
    //     _mapper = mapper;
    // }
    public ReviewReportService(IReviewReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
       
        _mapper = mapper;
    }
    public async Task<ApiResponse<ReviewReportResponse>> AddAsync(Guid reviewId, CreateReviewReportRequest request)
    {
       // var review = await _reviewRepository.GetByIdAsync(reviewId);
        // if (review == null)
        //     return ApiResponse<ReviewReportResponseDto>.Fail("Không tìm thấy review.");
        var r= _mapper.Map<ReviewReport>(request);

        await _reportRepository.AddAsync(r);

        var dto = _mapper.Map<ReviewReportResponse>(request);
        return ApiResponse<ReviewReportResponse>.Success(dto, "Tạo báo cáo thành công.");
    }

    public async Task<ApiResponse<ReviewReportResponse>> ApproveAsync(Guid reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            return ApiResponse<ReviewReportResponse>.Fail("Không tìm thấy báo cáo.");

        // report.Status = ReportStatus.Approved;
        // report.UpdatedAt = DateTime.UtcNow;
        // report.ReviewedAt = DateTime.UtcNow;
        //
        // // Cập nhật review (ẩn/xóa review tuỳ yêu cầu)
        // var review = await _reviewRepository.GetByIdAsync(report.ReviewId);
        // if (review != null)
        // {
        //     review.IsHidden = true; // hoặc review.Status = ReviewStatus.Hidden
        //     await _reviewRepository.UpdateAsync(review);
        // }

        await _reportRepository.UpdateAsync(report);

        var dto = _mapper.Map<ReviewReportResponse>(report);
        return ApiResponse<ReviewReportResponse>.Success(dto, "Đã duyệt báo cáo, review bị ẩn.");
    }

    public async Task<ApiResponse<ReviewReportResponse>> RejectAsync(Guid reportId, string reason)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            return ApiResponse<ReviewReportResponse>.Fail("Không tìm thấy báo cáo.");

        // report.Status = ReportStatus.Rejected;
        // report.Reason += $" | RejectReason: {reason}";
        // report.UpdatedAt = DateTime.UtcNow;
        // report.ReviewedAt = DateTime.UtcNow;

        await _reportRepository.UpdateAsync(report);

        var dto = _mapper.Map<ReviewReportResponse>(report);
        return ApiResponse<ReviewReportResponse>.Success(dto, "Đã từ chối báo cáo.");
    }

    public async Task<IEnumerable<ReviewReportResponse>> GetAllAsync()
    {
        var reports = await _reportRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ReviewReportResponse>>(reports);
    }
}
