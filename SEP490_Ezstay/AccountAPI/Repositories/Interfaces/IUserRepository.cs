using AccountAPI.Data;
using AccountAPI.DTO.Resquest;

namespace AccountAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User?> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(User user);
        Task<User> GetPhone(string phone);
        Task<User> GetCitizenIdNumber(string phone);

    }
}
