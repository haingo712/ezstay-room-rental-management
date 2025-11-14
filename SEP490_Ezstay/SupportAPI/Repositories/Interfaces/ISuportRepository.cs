using SupportAPI.Model;

namespace SupportAPI.Repositories.Interfaces
{
    public interface ISuportRepository
    {
        Task<List<SupportModel>> GetAllAsync();
        Task<SupportModel> GetByIdAsync(Guid id);
        Task CreateAsync(SupportModel support);
        Task UpdateAsync(SupportModel support);
    }
}
