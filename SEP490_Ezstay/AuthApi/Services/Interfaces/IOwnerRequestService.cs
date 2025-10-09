using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using System.Security.Claims;

namespace AuthApi.Services.Interfaces
{
    public interface IOwnerRequestService
    {
        Task<OwnerRequestResponseDto?> SubmitRequestAsync(SubmitOwnerRequestDto dto);
        Task<OwnerRequestResponseDto?> ApproveRequestAsync(Guid requestId, Guid staffId);
        Task<OwnerRequestResponseDto?> RejectRequestAsync(Guid requestId, Guid staffId, string rejectionReason);
        Task<List<OwnerRequestResponseDto>> GetAllRequestsAsync();


    }
}
