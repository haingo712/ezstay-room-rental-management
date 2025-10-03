using AccountAPI.DTO.Response;

namespace AccountAPI.Service.Interfaces
{
    public interface IAuthApiClient
    {
        Task<bool> ConfirmOtpAsync(string email, string otp);
        Task<bool> UpdateEmailAsync(string oldEmail, string newEmail);
        Task<AccountResponse?> GetByIdAsync(Guid id);
    }
}
