using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;

namespace ServiceAPI.Service.Interfaces
{
    public interface IServiceItemService
    {
        Task<ServiceItemResponseDto> CreateServiceAsync( ServiceItemRequestDto request);
        Task<List<ServiceItemResponseDto>> GetAllServicesAsync();
        Task<ServiceItemResponseDto> GetServiceByIdAsync(Guid id);
        Task UpdateServiceAsync(Guid id, ServiceItemRequestDto updatedService);
        Task DeleteServiceAsync(Guid id);
    }
}
