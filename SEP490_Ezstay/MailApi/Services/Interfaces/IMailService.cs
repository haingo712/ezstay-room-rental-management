
using MailApi.DTOs.Request;
using Shared.DTOs;
using Shared.DTOs.Mails.Response;

namespace MailApi.Services.Interfaces;
public interface IMailService
{
    Task<(bool success, string message)> VerifyOtp(Guid id, string otp , Guid signerId);
    Task<(bool success, string message,Guid? otpId)> SendOtp(string email, Guid contractId, Guid signerId);
    Task SendEmail(string toEmail, string subject, string body);
}
