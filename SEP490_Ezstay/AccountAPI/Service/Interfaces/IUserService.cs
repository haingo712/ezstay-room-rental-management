using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;

namespace AccountAPI.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto);
        Task<UserResponseDTO?> GetProfileAsync(Guid userId);
        Task<bool> UpdateProfileAsync(Guid userId, UserDTO userDto);
    }

}
