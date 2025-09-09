using AccountAPI.Data;

namespace AccountAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User?> GetByUserIdAsync(Guid userId);

    }
}
