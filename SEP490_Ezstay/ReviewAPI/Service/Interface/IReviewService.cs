using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Service.Interface;

public interface IReviewService
{
    Task<ApiResponse<IEnumerable<ReviewResponse>>> GetAll();
    IQueryable<ReviewResponse> GetAllAsQueryable();
  //  Task<ApiResponse<IEnumerable<ReviewResponse>>> GetAllByPostId(Guid postId);
    Task<ReviewResponse?> GetByContractIdAsync(Guid contractId);
    Task<ReviewResponse> GetByIdAsync(Guid id);
    //Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId,Guid postId, CreateReviewDto request);
    Task<ApiResponse<ReviewResponse>> AddAsync(Guid userId, Guid contractId, CreateReviewDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, Guid userId, UpdateReviewDto request);
    Task<ApiResponse<bool>> HideReview(Guid id, bool hidden);
    Task DeleteAsync(Guid id);
}