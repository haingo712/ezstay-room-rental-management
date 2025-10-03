using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IPostClientService
{
    Task<PostResponse?> GetByIdAsync(Guid postId);
    Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId);
}