using ServiceAPI.Model;

namespace ServiceAPI.Repositories.Interfaces
{
    public interface IServiceItemRepository
    {
        Task CreateServiceAsync(ServiceItem service);
        Task<List<ServiceItem>> GetAllServicesAsync();
        Task<ServiceItem> GetServiceByIdAsync(string id);
        Task UpdateServiceAsync(string id, ServiceItem updatedService);
        Task DeleteServiceAsync(string id);
    }
}
