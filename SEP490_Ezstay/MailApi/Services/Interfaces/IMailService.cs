namespace MailApi.Services.Interfaces;

public interface IMailService
{
    Task<(bool success, string message)> VerifyOtp(string email, string otp);
    Task SendOtp(string email, Guid? contractId);
    Task SendEmail(string toEmail, string subject, string body);
}
