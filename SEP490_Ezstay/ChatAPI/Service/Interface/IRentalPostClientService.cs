using Shared.DTOs.RentalPosts.Responses;

namespace ChatAPI.Service.Interface;

public interface IRentalPostClientService
{
    Task<RentalpostResponse?> GetByIdAsync(Guid postId);
}