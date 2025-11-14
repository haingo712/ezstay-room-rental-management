using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Service.Interface;

public interface IReviewService
{
    IQueryable<ReviewResponse> GetAll();
    Task<bool> ReviewExistsByContractId(Guid contractId);
    Task<ReviewResponse> GetById(Guid id);
    //Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid userId,Guid postId, CreateReviewDto request);
    Task<ApiResponse<ReviewResponse>> Add(Guid userId, Guid contractId, CreateReviewDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, Guid userId, UpdateReviewDto request);
    Task<ApiResponse<bool>> HideReview(Guid id, bool hidden);
    Task DeleteAsync(Guid id);
    Task<List<ReviewResponse>> GetByRoomIdsAsync(List<Guid> roomIds);
}
