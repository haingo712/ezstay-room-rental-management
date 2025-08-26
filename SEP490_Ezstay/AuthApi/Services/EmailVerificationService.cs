using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;

namespace AuthApi.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailVerificationRepository _repo;
        private readonly HttpClient _http;

        public EmailVerificationService(IEmailVerificationRepository repo, IHttpClientFactory factory)
        {
            _repo = repo;
            _http = factory.CreateClient("MailApi");
        }

        public async Task SendOtpAsync(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            var verification = new EmailVerification
            {
                Email = email,
                OtpCode = otp,
                ExpiredAt = DateTime.UtcNow.AddMinutes(1000)
            };

            await _repo.CreateAsync(verification);

            // Gửi OTP qua mail
            await _http.PostAsJsonAsync("/api/mail/send-verification", new
            {
                Email = email,
                Otp = otp
            });
        }

        public async Task<bool> ConfirmOtpAsync(string email, string otp)
        {
            return await _repo.VerifyOtpAsync(email, otp);
        }
    }
}
