using AuthApi.DTO.Request;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using System.Text.Json;

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

        public async Task SendOtpAsync(RegisterRequestDto dto)
        {
            // ❌ Bỏ dòng này đi
            // if (string.IsNullOrWhiteSpace(dto.Password))
            //     throw new ArgumentException("Password is required before sending OTP.");

            var otp = new Random().Next(100000, 999999).ToString();
            var payload = JsonSerializer.Serialize(dto);

            var verification = new EmailVerification
            {
                Email = dto.Email,
                OtpCode = otp,
                ExpiredAt = DateTime.UtcNow.AddMinutes(10),
                UserPayload = payload
            };

            await _repo.CreateAsync(verification);

            await _http.PostAsJsonAsync("/api/mail/send-verification", new
            {
                Email = dto.Email,
                Otp = otp
            });
        }

        public async Task SendOtpAsync(string userPayload)
        {
            var dto = JsonSerializer.Deserialize<RegisterRequestDto>(userPayload);
            if (dto != null)
            {
                await SendOtpAsync(dto);
            }
        }

        public async Task<EmailVerification?> ConfirmOtpAsync(string email, string otp)
        {
            return await _repo.VerifyOtpAsync(email, otp);
        }

        public async Task<EmailVerification> GetVerificationByEmail(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }

        public async Task SendResetPasswordEmailAsync(string email, string token)
        {
            var resetLink = $"https://your-frontend-url/reset-password?token={token}";

            await _http.PostAsJsonAsync("/api/mail/send", new
            {
                ToEmail = email,
                Subject = "Reset your password",
                Body = $"Click the link below to reset your password:\n{resetLink}"
            });
        }


        public async Task UpdateVerificationAsync(EmailVerification verification)
        {
            await _repo.UpdateAsync(verification);
        }

    }
}
