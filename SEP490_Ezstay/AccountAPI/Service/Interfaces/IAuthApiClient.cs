using AccountAPI.DTO.Response;
using AccountAPI.DTO.Resquest;

namespace AccountAPI.Service.Interfaces
{
    public interface IAuthApiClient
    {
        Task<bool> ConfirmOtpAsync(string email, string otp);
        Task<bool> UpdateEmailAsync(string oldEmail, string newEmail);
        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<bool> UpdateFullNameAsync(Guid id, string fullName);
        Task<ChangePasswordResponse?> ChangePasswordAsync(ChangePasswordRequest request);
     }
}
