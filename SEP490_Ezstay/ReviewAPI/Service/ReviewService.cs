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
    private readonly IContractClientService _contractClientService;
    private readonly IPostClientService _postClientService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IPostClientService PostClientService, IHttpContextAccessor httpContextAccessor, IContractClientService contractClientService)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _postClientService = PostClientService;
        _httpContextAccessor = httpContextAccessor;
        _contractClientService = contractClientService;
    }
    
    public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAll()
    {
        var entities = await _reviewRepository.GetAll();
        var dto = _mapper.Map<IEnumerable<ReviewResponseDto>>(entities);

        if (!dto.Any())
            return ApiResponse<IEnumerable<ReviewResponseDto>>.Fail("Không có review nào.");

        return ApiResponse<IEnumerable<ReviewResponseDto>>.Success(dto);
    }

    public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAllByPostId(Guid postId)
    {
        var entities = await _reviewRepository.GetAllByPostId(postId);
        var dto = _mapper.Map<IEnumerable<ReviewResponseDto>>(entities);
        if (!dto.Any())
            return ApiResponse<IEnumerable<ReviewResponseDto>>.Fail("Bài viết chưa có review nào.");
        return ApiResponse<IEnumerable<ReviewResponseDto>>.Success(dto);
    }

    // public IQueryable<ReviewDto> GetAllByPostIdAsQueryable(Guid postId)
    // {
    //     var entities = await _reviewRepository.GetAllByPostId(postId);
    //     var dto = _mapper.Map<IEnumerable<ReviewDto>>(entities);
    //     if (!dto.Any())
    //         return ApiResponse<IEnumerable<ReviewDto>>.Fail("Bài viết chưa có review nào.");
    //     return ApiResponse<IEnumerable<ReviewDto>>.Success(dto);
    // }
    public async Task<ReviewResponseDto> GetByIdAsync(Guid id)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        return _mapper.Map<ReviewResponseDto>(entity);
    }

    // public async Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId,Guid postId, CreateReviewDto request)
    // {
    //     
    //     // check user có hợp đồng chưa
    //     Console.WriteLine("dd "+ postId);
    //     var post = await _postClientService.GetByIdAsync(postId);
    //     Console.WriteLine("room "+post.RoomId);
    //     Console.WriteLine("y "+ userId);
    //     Console.WriteLine("dd "+ post.Id);
    //     var hasContract = await _contractClientService.CheckTenantHasContract(userId, post.RoomId);
    //     if (!hasContract)
    //         return ApiResponse<ReviewResponseDto>.Fail("Bạn chưa có hợp đồng, không thể tạo review.");
    //
    //     var review = _mapper.Map<Review>(request);
    //     review.UserId = userId;
    //     review.PostId = postId;
    //     review.CreatedAt = DateTime.UtcNow;
    //     await _reviewRepository.AddAsync(review);
    //     var dto = _mapper.Map<ReviewResponseDto>(review);
    //     return ApiResponse<ReviewResponseDto>.Success(dto, "Thêm review thành công");
    // }
    public async Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId, Guid contractId, CreateReviewDto request)
    {
        var contract = await _contractClientService.GetContractById(contractId);
        if (contract == null)
            return ApiResponse<ReviewResponseDto>.Fail("Không tìm thấy hợp đồng.");
        // if(contract. > DateTime.UtcNow)
        //     return  ApiResponse<ReviewResponseDto>.Fail("K dc qua"+ contract.CheckoutDate.AddMonths(1) +" ngay");
        if(contract.CheckoutDate.AddMonths(1) > DateTime.UtcNow)
            return  ApiResponse<ReviewResponseDto>.Fail("K dc qua"+ contract.CheckoutDate.AddMonths(1) +" ngay");
        var post =  await _postClientService.GetPostIdByRoomIdAsync(contract.RoomId);
        if (post == null)
            return ApiResponse<ReviewResponseDto>.Fail("Không tìm thấy bài đăng cho phòng này.");
        var review = _mapper.Map<Review>(request);
        review.ReviewDeadline = contract.CheckoutDate.AddMonths(1);
        review.UserId = userId;
        review.ContractId = contractId;
        review.PostId = post.Value;
        review.CreatedAt = DateTime.UtcNow;
        await _reviewRepository.AddAsync(review);
        var dto = _mapper.Map<ReviewResponseDto>(review);
        return ApiResponse<ReviewResponseDto>.Success(dto, "Thêm review thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id,Guid userId, UpdateReviewDto request)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review");
        
        if (entity.UserId != userId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật review này");
        if(entity.ReviewDeadline > DateTime.UtcNow)
            return  ApiResponse<bool>.Fail("K dc qua"+ entity.ReviewDeadline +" ngay");
        _mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.UpdateAsync(entity);

        return ApiResponse<bool>.Success(true, "Cập nhật review thành công");
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
    
        // var userId = GetUserIdFromToken();
        // if (entity.UserId != userId)
        //     throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
        await _reviewRepository.DeleteAsync(entity);
    }
}
