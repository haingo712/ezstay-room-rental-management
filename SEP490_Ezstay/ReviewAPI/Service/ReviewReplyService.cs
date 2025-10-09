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
    private readonly ITokenService _tokenService;
    private readonly IImageAPI _imageClient;

    public ReviewReplyService(IMapper mapper, IReviewReplyRepository reviewReplyRepository, ITokenService tokenService, IImageAPI imageClient)
    {
        _mapper = mapper;
        _reviewReplyRepository = reviewReplyRepository;
        _tokenService = tokenService;
        _imageClient = imageClient;
    }

    public IQueryable<ReviewReplyResponse> GetAllQueryable()
    => _reviewReplyRepository.GetAllQueryable().ProjectTo<ReviewReplyResponse>(_mapper.ConfigurationProvider);

    public async Task<ReviewReplyResponse?> GetByIdAsync(Guid id)
    { 
        return  _mapper.Map<ReviewReplyResponse>(await  _reviewReplyRepository.GetByIdAsync(id));
    }

    public async Task<ApiResponse<ReviewReplyResponse>> AddAsync(Guid reviewId, CreateReviewReplyRequest request)
    {
        var reviewReply = await _reviewReplyRepository.GetByIdAsync(reviewId);
        if (reviewReply == null)
            return ApiResponse<ReviewReplyResponse>.Fail("Không tìm thấy.");
        var reviewReplyDto = _mapper.Map<ReviewReply>(request);
        reviewReplyDto.CreatedAt = DateTime.UtcNow;
        reviewReplyDto.ReviewId = reviewId;
        _imageClient.UploadImageAsync(request.Image);
        await _reviewReplyRepository.AddAsync(reviewReply);
       
        var dto = _mapper.Map<ReviewReplyResponse>(reviewReply);
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
        _imageClient.UploadImageAsync(request.Image);
        return ApiResponse<bool>.Success(true, "Thêm ReviewReply thành công");
    }

    public async Task DeleteReplyAsync(Guid replyId)
    {
        var entity = await _reviewReplyRepository.GetByIdAsync(replyId);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        // var userId = GetUserIdFromToken();
        // if (entity.UserId != userId)
        //     throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
        await _reviewReplyRepository.DeleteAsync(entity.ReviewId);
    }
    
}