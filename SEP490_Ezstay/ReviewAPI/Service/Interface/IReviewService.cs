using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Service.Interface;

public interface IReviewService
{
   IQueryable<ReviewResponse> GetAll();
   IQueryable<ReviewResponse> GetAllByOwnerId(Guid ownerId);
    Task<bool> ReviewExistsByContractId(Guid contractId);
    Task<ReviewResponse> GetById(Guid id);
    Task<ApiResponse<ReviewResponse>> Add(Guid userId, Guid contractId, CreateReviewRequest request);
    Task<ApiResponse<bool>> Update(Guid id, UpdateReviewRequest request);
    Task<ApiResponse<bool>> HideReview(Guid id, bool hidden);
    
    IQueryable<ReviewResponse> GetAllReviewByRoomId(Guid roomId);
    
    
}
