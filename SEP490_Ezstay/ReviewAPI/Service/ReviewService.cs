using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Service;

public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // private Guid GetUserIdFromToken()
    // {
    //     var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (string.IsNullOrEmpty(userIdStr))
    //         throw new UnauthorizedAccessException("Không xác định được UserId từ token.");
    //     return Guid.Parse(userIdStr);
    // }

    public async Task<ApiResponse<IEnumerable<ReviewDto>>> GetAll()
    {
        var entities = await _reviewRepository.GetAll();
        var dto = _mapper.Map<IEnumerable<ReviewDto>>(entities);

        if (!dto.Any())
            return ApiResponse<IEnumerable<ReviewDto>>.Fail("Không có review nào.");

        return ApiResponse<IEnumerable<ReviewDto>>.Success(dto);
    }

    public async Task<ApiResponse<IEnumerable<ReviewDto>>> GetAllByPostId(Guid postId)
    {
        var entities = await _reviewRepository.GetAllByPostId(postId);
        var dto = _mapper.Map<IEnumerable<ReviewDto>>(entities);

        if (!dto.Any())
            return ApiResponse<IEnumerable<ReviewDto>>.Fail("Bài viết chưa có review nào.");

        return ApiResponse<IEnumerable<ReviewDto>>.Success(dto);
    }

    public async Task<ReviewDto> GetByIdAsync(Guid id)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");

        return _mapper.Map<ReviewDto>(entity);
    }

    public async Task<ApiResponse<ReviewDto>> AddAsync(Guid userId,Guid postId, CreateReviewDto request)
    {
      //   var userId = GetUserIdFromToken();
        var review = _mapper.Map<Review>(request);
        review.UserId = userId;
        review.PostId = postId;
        review.CreatedAt = DateTime.UtcNow;
        await _reviewRepository.AddAsync(review);
        var dto = _mapper.Map<ReviewDto>(review);
        return ApiResponse<ReviewDto>.Success(dto, "Thêm review thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id,Guid userId, UpdateReviewDto request)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review");

       // var userId = GetUserIdFromToken();
        if (entity.UserId != userId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật review này");
        
        _mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.UpdateAsync(entity);

        return ApiResponse<bool>.Success(true, "Cập nhật review thành công");
    }

    // public async Task DeleteAsync(Guid id)
    // {
    //     var entity = await _reviewRepository.GetByIdAsync(id);
    //     if (entity == null)
    //         throw new KeyNotFoundException("ReviewId not found");
    //
    //     var userId = GetUserIdFromToken();
    //     if (entity.UserId != userId)
    //         throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
    //
    //     await _reviewRepository.DeleteAsync(entity);
    // }
}
