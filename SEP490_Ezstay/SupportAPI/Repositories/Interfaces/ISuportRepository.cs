using SupportAPI.Model;

namespace SupportAPI.Repositories.Interfaces
{
    public interface ISuportRepository
    {
        Task<List<Support>> GetAllAsync();
        Task<Support> GetByIdAsync(Guid id);
        Task CreateAsync(Support support);
        Task UpdateAsync(Support support);
    }
}
