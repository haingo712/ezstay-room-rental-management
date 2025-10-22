using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Enum;
using ReviewReportAPI.Model;
using ReviewReportAPI.Repository.Interface;
using ReviewReportAPI.Service.Interface;
using Shared.DTOs;
namespace ReviewReportAPI.Service;

public class ReviewReportService : IReviewReportService
{
    private readonly IReviewReportRepository _reportRepository;
    private readonly IReviewClientService _reviewClientService;
    private readonly IImageClientService _imageClientService;
    private readonly IMapper _mapper;
    public ReviewReportService(IReviewReportRepository reportRepository, IReviewClientService reviewClientService, IMapper mapper, IImageClientService imageClientService)
    {
        _reportRepository = reportRepository;
        _reviewClientService = reviewClientService;
        _imageClientService = imageClientService;
        _mapper = mapper;
    }
    public  IQueryable<ReviewReportResponse> GetAll()
        => _reportRepository.GetAll().ProjectTo<ReviewReportResponse>(_mapper.ConfigurationProvider);


    public async Task<ReviewReportResponse> GetById(Guid id)
    {
        return _mapper.Map<ReviewReportResponse>(await _reportRepository.GetById(id));
    }
   
    public async Task<ApiResponse<ReviewReportResponse>> Add(Guid reviewId, CreateReviewReportRequest request)
    {
       // var review = await _reviewRepository.GetById(reviewId);
       //  if (review == null)
       //      return ApiResponse<ReviewReportResponseDto>.Fail("Không tìm thấy review.");
        var report= _mapper.Map<ReviewReport>(request);
        report.CreatedAt = DateTime.UtcNow;
        report.ReviewId = reviewId;
        report.Status = ReportStatus.Pending;
        //Console.WriteLine("ss "+ report.);
        report.Images =  await _imageClientService.UploadMultipleImage(request.Images);
        await _reportRepository.Add(report);
       
        var result = _mapper.Map<ReviewReportResponse>(report);
        return ApiResponse<ReviewReportResponse>.Success(result, "Tạo báo cáo thành công.");
    }
    public async Task<ApiResponse<ReviewReportResponse>> Update(Guid id, UpdateReviewReportRequest request)
    {
        var report = await _reportRepository.GetById(id);
         if (report == null)
             return ApiResponse<ReviewReportResponse>.Fail("Không tìm thấy review.");
         if (report.Status != ReportStatus.Pending)
             return ApiResponse<ReviewReportResponse>.Fail("Không dc cập nhật vì đơn này đã dc duyệt .");
        _mapper.Map(request, report);
        report.CreatedAt = DateTime.UtcNow;
        report.Status = ReportStatus.Pending;
        await _reportRepository.Update(report);
        await _imageClientService.UploadMultipleImage(request.Images);
        var dto = _mapper.Map<ReviewReportResponse>(report);
        return ApiResponse<ReviewReportResponse>.Success(dto, "Update báo cáo thành công.");
    }
    public async Task<ApiResponse<bool>> SetStatus(Guid reportId, UpdateReportStatusRequest request)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report == null)
            return ApiResponse<bool>.Fail("Không tìm thấy báo cáo.");
        _mapper.Map(request, report);
        report.ReviewedAt = DateTime.UtcNow;
        await _reportRepository.Update(report);
        if (request.Status == ReportStatus.Approved)
        {
            await _reviewClientService.HideReview(report.ReviewId, true);
            Console.WriteLine("cc"+ await _reviewClientService.HideReview(report.ReviewId, true));
        }
        else
        {
            await _reviewClientService.HideReview(report.ReviewId, false);
            Console.WriteLine("cccc"+ await _reviewClientService.HideReview(report.ReviewId, false));
        }

    
       // await _reviewClientService.HideReview(report.ReviewId, false);
        return ApiResponse<bool>.Success(true, "Set duyệt đơn thành công");
    }
}
