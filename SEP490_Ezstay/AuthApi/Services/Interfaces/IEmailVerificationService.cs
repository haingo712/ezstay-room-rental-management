namespace AuthApi.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task SendOtpAsync(string email);
        Task<bool> ConfirmOtpAsync(string email, string otp);
    }
}
