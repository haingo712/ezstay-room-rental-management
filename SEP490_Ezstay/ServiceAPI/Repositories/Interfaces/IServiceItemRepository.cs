using ServiceAPI.Model;

namespace ServiceAPI.Repositories.Interfaces
{
    public interface IServiceItemRepository
    {
        Task CreateServiceAsync(ServiceItem service);
        Task<List<ServiceItem>> GetAllServicesAsync();
        Task<ServiceItem> GetServiceByIdAsync(Guid id);
        Task UpdateServiceAsync(Guid id, ServiceItem updatedService);
        Task DeleteServiceAsync(Guid id);
    }
}
