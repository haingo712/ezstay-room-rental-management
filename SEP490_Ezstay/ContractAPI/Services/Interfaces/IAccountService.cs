using ContractAPI.DTO.Response;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.Services.Interfaces;

public interface IAccountService
{
    Task<ProfileResponse> GetProfileByUserId(Guid id );
}