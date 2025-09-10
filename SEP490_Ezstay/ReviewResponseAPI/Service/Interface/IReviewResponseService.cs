using ReviewResponseAPI.DTO.Requests;
using ReviewResponseAPI.DTO.Response;

namespace ReviewResponseAPI.Service.Interface;

public interface IReviewResponseService
{
    Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAllByOwnerId(Guid accountId);
    IQueryable<ReviewResponseDto> GetAllAsQueryable();
    Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAll();
    Task<ReviewResponseDto> GetByIdAsync(Guid id);
    Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid reviewId, Guid accountId,CreateReviewResponseDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,Guid accountId,  UpdateReviewResponseDto request);
    Task DeleteAsync(Guid id, Guid accountId);
}