using AutoMapper;
using AutoMapper.QueryableExtensions;
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
   
    private readonly IImageService _image;
    private readonly IReviewService _reviewService;

    public ReviewReplyService(IMapper mapper, IReviewReplyRepository reviewReplyRepository, IImageService image, IReviewService reviewService)
    {
        _mapper = mapper;
        _reviewReplyRepository = reviewReplyRepository;
        _image = image;
        _reviewService = reviewService;
    }

    public IQueryable<ReviewReplyResponse> GetAll()
    => _reviewReplyRepository.GetAll().ProjectTo<ReviewReplyResponse>(_mapper.ConfigurationProvider);

    public async Task<ReviewReplyResponse?> GetByIdAsync(Guid id)
    { 
        return  _mapper.Map<ReviewReplyResponse>(await  _reviewReplyRepository.GetById(id));
    }

    public async Task<ApiResponse<ReviewReplyResponse>> Add(Guid reviewId, Guid ownerId, CreateReviewReplyRequest request)
    {
        // var review = await _reviewService.GetById(reviewId);
        // if (review == null)
        //     return ApiResponse<ReviewReplyResponse>.Fail("Không tìm thấy.");
        var reviewReply = _mapper.Map<ReviewReply>(request);
        reviewReply.CreatedAt = DateTime.UtcNow;
        reviewReply.ReviewId = reviewId;
        reviewReply.OwnerId = ownerId;
        _image.UploadMultipleImage(request.Image);
        await _reviewReplyRepository.Add(reviewReply);
       
        var dto = _mapper.Map<ReviewReplyResponse>(reviewReply);
        return ApiResponse<ReviewReplyResponse>.Success(dto, "Reply Successfully");
    }

    public async Task<ReviewReplyResponse> GetReplyByReviewIdAsync(Guid reviewId)
    {
       var review =  await  _reviewReplyRepository.GetByReviewId(reviewId);
      return  _mapper.Map<ReviewReplyResponse>(review);
    }

    public async Task<ApiResponse<bool>> UpdateReplyAsync(Guid id, UpdateReviewReplyRequest request)
    {
        var reviewReply = await _reviewReplyRepository.GetById(id);
        if (reviewReply == null)
            throw new KeyNotFoundException("ReviewId not found");
        _mapper.Map(request, reviewReply);
        reviewReply.UpdatedAt = DateTime.UtcNow;
       
        _reviewReplyRepository.Update(reviewReply);
        _image.UploadMultipleImage(request.Image);
        return ApiResponse<bool>.Success(true, "Update Successfully");
    }

    public async Task Delete(Guid id)
    {
        // var entity = await _reviewReplyRepository.GetById(id);
        // if (entity == null)
        //     throw new KeyNotFoundException("ReviewId not found");
        // var userId = GetUserIdFromToken();
        // if (entity.UserId != userId)
        //     throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
        await _reviewReplyRepository.Delete(id);
    }
    
}