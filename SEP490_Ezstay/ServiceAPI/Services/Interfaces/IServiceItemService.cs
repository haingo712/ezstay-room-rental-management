using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;

namespace ServiceAPI.Service.Interfaces
{
    public interface IServiceItemService
    {
        Task<ServiceItemResponseDto> CreateServiceAsync(ServiceItemRequestDto request);
        Task<List<ServiceItemResponseDto>> GetAllServicesAsync();
        Task<ServiceItemResponseDto> GetServiceByIdAsync(string id);
        Task UpdateServiceAsync(string id, ServiceItemRequestDto updatedService);
        Task DeleteServiceAsync(string id);
    }
}
