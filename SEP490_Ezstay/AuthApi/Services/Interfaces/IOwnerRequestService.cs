using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace AuthApi.Services.Interfaces
{
    public interface IOwnerRequestService
    {
        Task<OwnerRequestResponseDto?> SubmitRequestAsync(SubmitOwnerRequestDto dto);
        Task<string> ApproveRequestAsync(Guid requestId);
    }
}
