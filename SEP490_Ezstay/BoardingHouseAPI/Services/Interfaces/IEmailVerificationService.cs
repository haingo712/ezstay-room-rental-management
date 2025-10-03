using AuthApi.DTO.Request;
using AuthApi.Models;

namespace AuthApi.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task SendOtpAsync(RegisterRequestDto dto);
        Task SendOtpAsync(string userPayload);
        Task<EmailVerification?> ConfirmOtpAsync(string email, string otp);
        Task<EmailVerification> GetVerificationByEmail(string email);
        Task SendResetPasswordEmailAsync(string email, string token);
    }
}
