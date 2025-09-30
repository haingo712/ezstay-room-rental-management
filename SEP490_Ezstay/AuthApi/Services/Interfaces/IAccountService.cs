using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace AuthApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAsync(AccountRequest request);

        // Lấy account theo id, tự kiểm tra quyền bên trong service
        Task<AccountResponse?> GetByIdAsync(Guid id);

        // Lấy tất cả account, tự lọc Admin nếu user là Staff
        Task<List<AccountResponse>> GetAllAsync();

        Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request);
        Task VerifyAsync(string email);
        Task BanAsync(Guid id);
        Task UnbanAsync(Guid id);
    }
}

