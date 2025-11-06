using MailApi.Model;

namespace MailApi.Repository.Interface;

public interface IMailRepository
{
    Task<OtpVerification?> GetByEmailAndCodeAsync(string email, string otp);
    Task<OtpVerification?> GetByIdAsync(Guid id);
    Task<OtpVerification> AddAsync(OtpVerification otpVerification);
    Task Update(OtpVerification otp);
    Task DeleteAsync(OtpVerification otpVerification);
}