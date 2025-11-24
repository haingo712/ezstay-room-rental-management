
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.DTO.Response;
using System.Security.Claims;


namespace RentalPostsAPI.Service.Interface
{
    public interface IRentalPostService
    {
        IQueryable<RentalpostDTO> GetAllAsQueryable();
        Task<ApiResponse<RentalpostDTO>> CreateAsync(CreateRentalPostDTO dto, ClaimsPrincipal user);
        Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync();
        Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user);
        Task<RentalpostDTO?> GetByIdAsync(Guid id);
        Task<RentalpostDTO?> UpdateAsync(Guid id, UpdateRentalPostDTO dto);
        Task<bool> DeleteAsync(Guid id, Guid deletedBy);
        Task<IEnumerable<RentalpostDTO>> GetPendingPostsAsync();
        Task<bool> ApprovePostAsync(Guid postId, Guid staffId);
        Task<bool> RejectPostAsync(Guid postId, Guid staffId);
        Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId);
        // Task<IEnumerable<RentalpostDTO>> GetByRoomIdAsync(Guid roomId);
    }
}
