using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace UserManagerAPI.Service.Interfaces
{
    public interface IAccountApiClient
    {
        Task<List<AccountResponse>?> GetAllAsync();
        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<AccountResponse?> CreateAsync(AccountRequest request);
        Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request);
        Task BanAsync(Guid id);
        Task UnbanAsync(Guid id);
    }
}