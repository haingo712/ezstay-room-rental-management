using AuthApi.DTO.Request;

namespace AuthApi.Services.Interfaces
{
    public interface IOwnerRequestService
    {
        Task<string> SubmitRequestAsync(string email, SubmitOwnerRequestDto dto);
        Task<string> ApproveRequestAsync(Guid requestId);
    }
}
