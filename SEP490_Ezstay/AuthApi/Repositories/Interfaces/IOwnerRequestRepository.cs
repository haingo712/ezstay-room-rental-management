using AuthApi.Models;

namespace AuthApi.Repositories.Interfaces
{
    public interface IOwnerRequestRepository
    {
        Task CreateAsync(OwnerRegistrationRequest request);
        Task<OwnerRegistrationRequest?> GetByIdAsync(Guid id);
        Task UpdateAsync(OwnerRegistrationRequest request);
        Task<List<OwnerRegistrationRequest>> GetAllAsync();
    }

}
