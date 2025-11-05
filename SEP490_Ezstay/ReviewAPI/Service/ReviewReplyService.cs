using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewAPI.APIs.Interfaces;
using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service.Interface;
using Shared.DTOs;

namespace ReviewAPI.Service;

public class ReviewReplyService: IReviewReplyService
{
    private readonly IMapper _mapper;
    private readonly IReviewReplyRepository _reviewReplyRepository;
   
    private readonly IImageClientService _imageClient;
    private readonly IReviewService _reviewService;

    public ReviewReplyService(IMapper mapper, IReviewReplyRepository reviewReplyRepository, IImageClientService imageClient, IReviewService reviewService)
    {
        _mapper = mapper;
        _reviewReplyRepository = reviewReplyRepository;
        _imageClient = imageClient;
        _reviewService = reviewService;
    }

    public IQueryable<ReviewReplyResponse> GetAllQueryable()
    => _reviewReplyRepository.GetAllQueryable().ProjectTo<ReviewReplyResponse>(_mapper.ConfigurationProvider);

    public async Task<ReviewReplyResponse?> GetByIdAsync(Guid id)
    { 
        return  _mapper.Map<ReviewReplyResponse>(await  _reviewReplyRepository.GetByIdAsync(id));
    }

    public async Task<ApiResponse<ReviewReplyResponse>> AddAsync(Guid reviewId, Guid ownerId, CreateReviewReplyRequest request)
    {
        var review = await _reviewService.GetByIdAsync(reviewId);
        if (review == null)
            return ApiResponse<ReviewReplyResponse>.Fail("Không tìm thấy.");
        var reviewReplyDto = _mapper.Map<ReviewReply>(request);
        reviewReplyDto.CreatedAt = DateTime.UtcNow;
        reviewReplyDto.ReviewId = reviewId;
        reviewReplyDto.OwnerId = ownerId;
        _imageClient.UploadMultipleImage(request.Image);
        await _reviewReplyRepository.AddAsync(reviewReplyDto);
       
        var dto = _mapper.Map<ReviewReplyResponse>(reviewReplyDto);
        return ApiResponse<ReviewReplyResponse>.Success(dto, "Thêm ReviewReply thành công");
    }

    public async Task<ReviewReplyResponse> GetReplyByReviewIdAsync(Guid reviewId)
    {
       var review =  await  _reviewReplyRepository.GetByReviewIdAsync(reviewId);
      return  _mapper.Map<ReviewReplyResponse>(review);
    }

    public async Task<ApiResponse<bool>> UpdateReplyAsync(Guid id, UpdateReviewReplyRequest request)
    {
        var entity = await _reviewReplyRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        _mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
       
        _reviewReplyRepository.UpdateAsync(entity);
        _imageClient.UploadMultipleImage(request.Image);
        return ApiResponse<bool>.Success(true, "Thêm ReviewReply thành công");
    }

    public async Task DeleteReplyAsync(Guid replyId)
    {
        var entity = await _reviewReplyRepository.GetByIdAsync(replyId);
      //  Console.WriteLine("eeee "+ entity.ReviewId+ "  "+entity);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        
        // var userId = GetUserIdFromToken();
        // if (entity.UserId != userId)
        //     throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
        await _reviewReplyRepository.DeleteAsync(replyId);
    }
    
}