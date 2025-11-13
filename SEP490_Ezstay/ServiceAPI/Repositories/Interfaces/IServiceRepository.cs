namespace ServiceAPI.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task CreateServiceAsync(Model.ServiceModel service);
        Task<List<Model.ServiceModel>> GetAllServicesAsync();
        Task<Model.ServiceModel> GetServiceByIdAsync(string id);
        Task UpdateServiceAsync(string id, Model.ServiceModel updatedService);
        Task DeleteServiceAsync(string id);
    }
}
