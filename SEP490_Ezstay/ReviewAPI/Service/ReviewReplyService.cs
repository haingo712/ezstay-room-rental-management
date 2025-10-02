using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Service;

public class ReviewReplyService: IReviewReplyService
{
    private readonly IMapper _mapper;
    private readonly IReviewReplyRepository _reviewReplyRepository;
    private readonly ITokenService _tokenService;
    public ReviewReplyService(IMapper mapper, IReviewReplyRepository reviewReplyRepository, ITokenService tokenService)
    {
        _mapper = mapper;
        _reviewReplyRepository = reviewReplyRepository;
        _tokenService = tokenService;
    }

    public IQueryable<ReviewReplyResponse> GetAllQueryable()
    => _reviewReplyRepository.GetAllQueryable().ProjectTo<ReviewReplyResponse>(_mapper.ConfigurationProvider);

    public async Task<ReviewReplyResponse?> GetByIdAsync(Guid id)
    { 
        return  _mapper.Map<ReviewReplyResponse>(await  _reviewReplyRepository.GetByIdAsync(id));
    }

    public async Task<ApiResponse<ReviewReplyResponse>> AddAsync(Guid reviewId, CreateReviewReplyRequest reviewReplyRequest)
    {
        var review = await _reviewReplyRepository.GetByIdAsync(reviewId);
        if (review == null)
            return ApiResponse<ReviewReplyResponse>.Fail("Không tìm thấy.");
        var reviewDto = _mapper.Map<ReviewReply>(reviewReplyRequest);
        reviewDto.CreatedAt = DateTime.UtcNow;
        await _reviewReplyRepository.AddAsync(review);
        var dto = _mapper.Map<ReviewReplyResponse>(review);
        return ApiResponse<ReviewReplyResponse>.Success(dto, "Thêm ReviewReply thành công");
    }

    public async Task<ReviewReplyResponse> GetReplyByReviewIdAsync(Guid reviewId)
    {
       var review =  await  _reviewReplyRepository.GetByReviewIdAsync(reviewId);
      return  _mapper.Map<ReviewReplyResponse>(review);
    }

    public async Task<ApiResponse<bool>> UpdateReplyAsync(Guid replyId, UpdateReviewReplyRequest reviewReply)
    {
        var entity = await _reviewReplyRepository.GetByIdAsync(replyId);
        if (entity == null)
            throw new KeyNotFoundException("ReviewId not found");
        _mapper.Map(reviewReply, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        var dto=  _reviewReplyRepository.UpdateAsync(entity);
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