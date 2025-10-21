using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewAPI.APIs.Interfaces;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Service;

public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    private readonly IReviewRepository _reviewRepository;
    private readonly IContractClientService _contractClientService;
    private readonly IPostClientService _postClientService;
    private readonly IImageAPI _imageClient;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IContractClientService contractClientService, IPostClientService postClientService, IImageAPI imageClient)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _contractClientService = contractClientService;
        _postClientService = postClientService;
        _imageClient = imageClient;
    }

    public IQueryable<ReviewResponse> GetAllAsQueryable()
    {
        var post = _reviewRepository.GetAllAsQueryable();
        return post.ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider);
    }
    
    public async Task<ApiResponse<IEnumerable<ReviewResponse>>> GetAll()
    {
        var entities = await _reviewRepository.GetAll();
        var dto = _mapper.Map<IEnumerable<ReviewResponse>>(entities);

        if (!dto.Any())
            return ApiResponse<IEnumerable<ReviewResponse>>.Fail("Không có review nào.");

        return ApiResponse<IEnumerable<ReviewResponse>>.Success(dto);
    }

    // public async Task<ApiResponse<IEnumerable<ReviewResponse>>> GetAllByPostId(Guid postId)
    // {
    //     var entities = await _reviewRepository.GetAllByPostId(postId);
    //     var dto = _mapper.Map<IEnumerable<ReviewResponse>>(entities);
    //     if (!dto.Any())
    //         return ApiResponse<IEnumerable<ReviewResponse>>.Fail("Bài viết chưa có review nào.");
    //     return ApiResponse<IEnumerable<ReviewResponse>>.Success(dto);
    // }

    // public IQueryable<ReviewDto> GetAllByPostIdAsQueryable(Guid postId)
    // {
    //     var entities = await _reviewRepository.GetAllByPostId(postId);
    //     var dto = _mapper.Map<IEnumerable<ReviewDto>>(entities);
    //     if (!dto.Any())
    //         return ApiResponse<IEnumerable<ReviewDto>>.Fail("Bài viết chưa có review nào.");
    //     return ApiResponse<IEnumerable<ReviewDto>>.Success(dto);
    // }
    public async Task<ReviewResponse> GetByIdAsync(Guid id)
    {
        var entity = await _reviewRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        return _mapper.Map<ReviewResponse>(entity);
    }

    public async Task<ReviewResponse?> GetByContractIdAsync(Guid contractId)
    {
        var entity = await _reviewRepository.GetByContractIdAsync(contractId);
        return _mapper.Map<ReviewResponse>(entity);
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
    public async Task<ApiResponse<ReviewResponse>> AddAsync(Guid userId, Guid contractId, CreateReviewDto request)
    {
        var contract = await _contractClientService.GetContractById(contractId);
        if (contract == null)
            return ApiResponse<ReviewResponse>.Fail("Không tìm thấy hợp đồng.");
        // var existingReview = await _reviewRepository.GetByContractIdAsync(contractId);
        // if (existingReview != null)
        //     return ApiResponse<ReviewResponse>.Fail("Hợp đồng này đã được review, không thể review thêm.");
        if(contract.CheckoutDate.AddMonths(1) < DateTime.UtcNow)
            return  ApiResponse<ReviewResponse>.Fail("K dc qua"+ contract.CheckoutDate.AddMonths(1) +" ngay");
        // var post =  await _postClientService.GetPostIdByRoomIdAsync(contract.RoomId);
        // if (post == null)
        //     return ApiResponse<ReviewResponse>.Fail("Không tìm thấy bài đăng cho phòng này.");
        var review = _mapper.Map<Review>(request);
        review.ReviewDeadline = contract.CheckoutDate.AddMonths(1);
        review.UserId = userId;
        review.ContractId = contractId;
        review.RoomId= contract.RoomId;
    //    review.PostId = post.Value;
        review.CreatedAt = DateTime.UtcNow;
        review.ImageUrl =  _imageClient.UploadImageAsync(request.ImageUrl).Result;
        await _reviewRepository.AddAsync(review);
        var dto = _mapper.Map<ReviewResponse>(review);
        return ApiResponse<ReviewResponse>.Success(dto, "Thêm review thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id,Guid userId, UpdateReviewDto request)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review");
        
        if (review.UserId != userId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật review này");
        if(review.ReviewDeadline < DateTime.UtcNow)
            return  ApiResponse<bool>.Fail("K dc qua"+ review.ReviewDeadline +" ngay");
        _mapper.Map(request, review);
        review.UpdatedAt = DateTime.UtcNow;
        review.ImageUrl =  _imageClient.UploadImageAsync(request.ImageUrl).Result;
        await _reviewRepository.UpdateAsync(review);

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
