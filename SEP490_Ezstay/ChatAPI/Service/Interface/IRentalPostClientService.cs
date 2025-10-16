using Shared.DTOs.RentalPosts.Responses;

namespace ChatAPI.Service.Interface;

public interface IRentalPostClientService
{
    Task<RentalPostResponse?> GetById(Guid postId);
}