using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using Auths.Responses;

namespace AuthApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<RegisterResponseDto> ResendOtpAsync(string email);
        Task<RegisterResponseDto> CreateStaffAsync(CreateStaffRequestDto dto);
        Task<RegisterResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto);
        Task<RegisterResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<RegisterResponseDto> SendPhoneOtpAsync(string phone);
        Task<RegisterResponseDto> VerifyPhoneOtpAsync(string phone, string otp);
        Task<RegisterResponseDto> ConfirmOtpForForgotPasswordAsync(string email, string otp);
        }
}
