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
    private readonly IContractService _contractService;
    private readonly IRentalPostService _rentalPostService;
    private readonly IImageService _image;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IContractService contractService, IRentalPostService rentalPostService, IImageService image)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _contractService = contractService;
        _rentalPostService = rentalPostService;
        _image = image;
    }

    public IQueryable<ReviewResponse> GetAll()
    {
        var post = _reviewRepository.GetAll();
        return post.ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider);
    }
    
    // public IQueryable<ReviewDto> GetAllByPostIdAsQueryable(Guid postId)
    // {
    //     var entities = await _reviewRepository.GetAllByPostId(postId);
    //     var dto = _mapper.Map<IEnumerable<ReviewDto>>(entities);
    //     if (!dto.Any())
    //         return ApiResponse<IEnumerable<ReviewDto>>.Fail("Bài viết chưa có review nào.");
    //     return ApiResponse<IEnumerable<ReviewDto>>.Success(dto);
    // }
    public async Task<ReviewResponse> GetById(Guid id)
    {
        var entity = await _reviewRepository.GetById(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        return _mapper.Map<ReviewResponse>(entity);
    }

    public async Task<bool> ReviewExistsByContractId(Guid contractId) 
        => await _reviewRepository.ReviewExistsByContractId(contractId);
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
    public async Task<ApiResponse<ReviewResponse>> Add(Guid userId, Guid contractId, CreateReviewDto request)
    {
        var contract = await _contractService.GetContractId(contractId);
        if (contract == null)
            return ApiResponse<ReviewResponse>.Fail("Không tìm thấy hợp đồng.");
        var existingReview = await _reviewRepository.ReviewExistsByContractId(contractId);
        if (existingReview)
            return ApiResponse<ReviewResponse>.Fail("Contract Already has review.");
        if (contract.CheckoutDate.AddMonths(1) < DateTime.UtcNow)
            return ApiResponse<ReviewResponse>.Fail("K dc qua" + contract.CheckoutDate.AddMonths(1) + " ngay");
        var review = _mapper.Map<Review>(request);
        review.ReviewDeadline = contract.CheckoutDate.AddMonths(1);
        review.UserId = userId;
        review.ContractId = contractId;
        review.RoomId = contract.RoomId;
        review.IsHidden = false;
        review.CreatedAt = DateTime.UtcNow;
        review.ImageUrl = _image.UploadMultipleImage(request.ImageUrl).Result;
        await _reviewRepository.Add(review);
        var dto = _mapper.Map<ReviewResponse>(review);
        return ApiResponse<ReviewResponse>.Success(dto, "Thêm review thành công");
    }

    public async Task<ApiResponse<bool>> Update(Guid id, Guid userId, UpdateReviewDto request)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review");
        if (review.UserId != userId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật review này");
        if (review.ReviewDeadline < DateTime.UtcNow)
            return ApiResponse<bool>.Fail("K dc qua" + review.ReviewDeadline + " ngay");
        _mapper.Map(request, review);
        review.UpdatedAt = DateTime.UtcNow;
        review.ImageUrl = _image.UploadMultipleImage(request.ImageUrl).Result;
        await _reviewRepository.Update(review);

        return ApiResponse<bool>.Success(true, "Cập nhật review thành công");
    }
    public async Task<ApiResponse<bool>> HideReview(Guid id, bool hidden)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review.");
        review.IsHidden = hidden;
        review.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.Update(review);
        return ApiResponse<bool>.Success(true, "Ẩn review thành công.");
    }


    public async Task Delete(Guid id)
    {
        var entity = await _reviewRepository.GetById(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        // var userId = GetUserIdFromToken();
        // if (entity.UserId != userId)
        //     throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
        await _reviewRepository.Delete(entity);
    }

    public async Task<List<ReviewResponse>> GetByRoomIdsAsync(List<Guid> roomIds)
    {
        var reviews = await _reviewRepository.GetByRoomIds(roomIds);
        return _mapper.Map<List<ReviewResponse>>(reviews);
    }
}
