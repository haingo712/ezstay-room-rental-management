using AccountAPI.Data;
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
        Task<bool> UpdateProfile(Guid userId, UpdateUserDTO userDto);
        Task<bool> UpdatePhoneAsync(Guid userId, string newPhone);
        Task<bool> VerifyPhoneOtpAsync(string phone, string otp);
        Task<bool> SendOtpToPhoneAsync(string phone);
        Task<bool> UpdateEmailAsync(string currentEmail, string newEmail, string otp);
        Task<UserResponseDTO> GetPhone(string phone);
        Task<UserResponseDTO> GetCitizenIdNumber(string citizenIdNumber);
        Task<bool> CheckProfileAsync(Guid id);



    }

}
