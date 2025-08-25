using AuthApi.Models;

namespace AuthApi.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByPhoneAsync(string phone);
        Task CreateAsync(Account account);
        Task MarkAsVerified(string email);
    }
}
