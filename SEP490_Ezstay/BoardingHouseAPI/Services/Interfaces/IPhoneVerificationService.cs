namespace AuthApi.Services.Interfaces
{
    public interface IPhoneVerificationService
    {
        Task SendOtpAsync(string phone);
        Task<bool> VerifyOtpAsync(string phone, string otp);
    }
}
