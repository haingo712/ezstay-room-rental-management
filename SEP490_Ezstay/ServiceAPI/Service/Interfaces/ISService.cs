using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;

namespace ServiceAPI.Service.Interfaces
{
    public interface ISService
    {
        Task<ServiceResponseDto> CreateServiceAsync(ServiceRequestDto request);
        Task<List<ServiceResponseDto>> GetAllServicesAsync();
        Task<ServiceResponseDto> GetServiceByIdAsync(string id);
        Task UpdateServiceAsync(string id, ServiceRequestDto updatedService);
        Task DeleteServiceAsync(string id);
    }
}
