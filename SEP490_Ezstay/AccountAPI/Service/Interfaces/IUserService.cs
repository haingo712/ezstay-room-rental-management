using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using System.Security.Claims;

namespace AccountAPI.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto);
        Task<UserResponseDTO?> GetProfileAsync(Guid userId);
        Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDTO dto, ClaimsPrincipal user);

        Task<bool> UpdatePhoneAsync(Guid userId, string newPhone);
        Task<bool> VerifyPhoneOtpAsync(string phone, string otp);
        Task<bool> SendOtpToPhoneAsync(string phone);



    }

}
