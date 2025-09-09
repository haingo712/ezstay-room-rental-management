using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IReviewService
{
    Task<ApiResponse<IEnumerable<ReviewDto>>> GetAll();
    Task<ApiResponse<IEnumerable<ReviewDto>>> GetAllByPostId(Guid postId);
    Task<ReviewDto> GetByIdAsync(Guid id);
    Task<ApiResponse<ReviewDto>> AddAsync(Guid postId, CreateReviewDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateReviewDto request);
   // Task DeleteAsync(Guid id);
}