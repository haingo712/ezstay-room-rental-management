using AuthApi.Models;

namespace AuthApi.Repositories.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task CreateAsync(EmailVerification verification);
        Task<EmailVerification?> GetByEmailAsync(string email);
        Task<EmailVerification?> VerifyOtpAsync(string email, string otp);
    }
}
