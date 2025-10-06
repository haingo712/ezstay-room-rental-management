using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IReviewService
{
    Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAll();
    IQueryable<ReviewResponseDto> GetAllAsQueryable();
    Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAllByPostId(Guid postId);
    Task<ReviewResponseDto?> GetByContractIdAsync(Guid contractId);
    Task<ReviewResponseDto> GetByIdAsync(Guid id);
    //Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId,Guid postId, CreateReviewDto request);
    Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId, Guid contractId, CreateReviewDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, Guid userId, UpdateReviewDto request);
    Task DeleteAsync(Guid id);
}