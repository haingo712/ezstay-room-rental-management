using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;

namespace AccountAPI.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto);
        Task<UserResponseDTO?> GetProfileAsync(Guid userId);
    }

}
