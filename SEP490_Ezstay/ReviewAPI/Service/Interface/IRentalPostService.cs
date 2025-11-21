using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IRentalPostService
{
    Task<PostResponse?> GetByIdAsync(Guid postId);
    Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId);
}