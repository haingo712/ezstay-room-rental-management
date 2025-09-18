namespace AccountAPI.Service.Interfaces
{
    public interface IPhoneOtpClient
    {
        Task<bool> SendOtpAsync(string phone);
        Task<bool> VerifyOtpAsync(string phone, string otp);
    }
}
