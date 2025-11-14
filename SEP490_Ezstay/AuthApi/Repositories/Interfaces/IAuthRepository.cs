
using AuthApi.Models;
using Shared.Enums;

namespace AuthApi.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByPhoneAsync(string phone);
        Task<Account?> GetByIdAsync(Guid id);
        Task<List<Account>> GetAllAsync();
        Task CreateAsync(Account account);
        Task<Account?> UpdateAsync(Account account);
        Task MarkAsVerified(string email);
        Task BanAccountAsync(Guid id, bool isBanned);
        Task<List<Account>> GetByRoleAsync(RoleEnum role);
        Task AddAsync(Account account) ;
    }
}
