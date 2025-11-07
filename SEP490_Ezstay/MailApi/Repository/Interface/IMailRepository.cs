using MailApi.Model;

namespace MailApi.Repository.Interface;

public interface IMailRepository
{
    /// <summary>
    /// Get OTP by ContractId, OTP code, and Actor - more accurate than email
    /// </summary>
    Task<OtpVerification?> GetByContractAndCodeAsync(Guid contractId, string otp);
    Task<OtpVerification?> GetByContractAndSignerId(Guid contractId, string otp, Guid signerId);
    Task<OtpVerification?> GetByEmailAndCodeAsync(string email, string otp, Guid? contractId = null);
    Task<OtpVerification?> GetByIdAsync(Guid id);
    Task<OtpVerification> AddAsync(OtpVerification otpVerification);
    Task Update(OtpVerification otp);
}