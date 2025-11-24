using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IImageService _imageService;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IContractService contractService, IImageService imageService)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _contractService = contractService;
        _imageService = imageService;
    }

    // public IQueryable<ReviewResponse> GetAll()
    // {
    //     var post = _reviewRepository.GetAll();
    //     return post.ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider);
    // }
    
    public async Task<ReviewResponse> GetById(Guid id)
    {
        var entity = await _reviewRepository.GetById(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        return _mapper.Map<ReviewResponse>(entity);
    }

    public IQueryable<ReviewResponse> GetAllByOwnerId(Guid ownerId)
    {
        var post = _reviewRepository.GetAllByOwnerId(ownerId);
        return post.ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider);
    }

    public async Task<bool> ReviewExistsByContractId(Guid contractId) 
        => await _reviewRepository.ReviewExistsByContractId(contractId);
   
    public async Task<ApiResponse<ReviewResponse>> Add(Guid userId, Guid contractId, CreateReviewRequest request)
    {
        var contract = await _contractService.GetContractId(contractId);
        // if (contract == null)
        //     return ApiResponse<ReviewResponse>.Fail("Không tìm thấy hợp đồng.");
        var existingReview = await _reviewRepository.ReviewExistsByContractId(contractId);
        if (existingReview)
            return ApiResponse<ReviewResponse>.Fail("Contract Already has review.");
        if (contract.CheckoutDate.AddMonths(1) < DateTime.UtcNow)
            return ApiResponse<ReviewResponse>.Fail("It has been over"+contract.CheckoutDate.AddMonths(1) +" month since the contract expired and cannot be reviewed.");
        var review = _mapper.Map<Review>(request);
        
        review.OwnerId = contract.IdentityProfiles.First().UserId;
        review.ReviewDeadline = contract.CheckoutDate.AddMonths(1);
        review.UserId = userId;
        review.ContractId = contractId;
        review.RoomId = contract.RoomId;
        review.IsHidden = false;
        review.CreatedAt = DateTime.UtcNow;
        review.ImageUrl = _imageService.UploadMultipleImage(request.ImageUrl).Result;
        await _reviewRepository.Add(review);
        var dto = _mapper.Map<ReviewResponse>(review);
        return ApiResponse<ReviewResponse>.Success(dto, "Created Successfully");
    }

    public async Task<ApiResponse<bool>> Update(Guid id, Guid userId, UpdateReviewRequest request)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review");
        // if (review.UserId != userId)
        //     return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật review này");
        if (review.ReviewDeadline < DateTime.UtcNow)
            return ApiResponse<bool>.Fail("K dc qua" + review.ReviewDeadline + " ngay");
        _mapper.Map(request, review);
        review.UpdatedAt = DateTime.UtcNow;
        review.ImageUrl = _imageService.UploadMultipleImage(request.ImageUrl).Result;
        await _reviewRepository.Update(review);

        return ApiResponse<bool>.Success(true, "Update Successfully");
    }
    public async Task<ApiResponse<bool>> HideReview(Guid id, bool hidden)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
            return ApiResponse<bool>.Fail("Không tìm thấy review.");
        review.IsHidden = hidden;
        review.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.Update(review);
        return ApiResponse<bool>.Success(true, "Hide Successfully");
    }
    
    public async Task<List<ReviewResponse>> GetByRoomIdsAsync(List<Guid> roomIds)
    {
        var reviews = await _reviewRepository.GetByRoomIds(roomIds);
        return _mapper.Map<List<ReviewResponse>>(reviews);
    }
}
