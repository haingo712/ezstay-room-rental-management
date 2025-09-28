using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace AuthApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAsync(AccountRequest request);
        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<List<AccountResponse>> GetAllAsync();   // ✅ bỏ string role
        Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request);
        Task VerifyAsync(string email);
        Task BanAsync(Guid id);
        Task UnbanAsync(Guid id);
    }
}

