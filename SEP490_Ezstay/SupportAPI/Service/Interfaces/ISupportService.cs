using SupportAPI.DTO.Request;
using SupportAPI.DTO.Response;

namespace SupportAPI.Service.Interfaces
{
    public interface ISupportService
    {
        Task<List<SupportResponse>> GetAllAsync();
        Task<SupportResponse> CreateAsync(CreateSupportRequest request);
        Task<SupportResponse> UpdateStatusAsync(Guid id, UpdateSupportStatusRequest request);
     
    }
}
