using ContractAPI.DTO.Response;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.APIs.Interfaces;

public interface IAccountAPI
{
    Task<IdentityProfileResponse?> GetProfileByPhoneAsync(string phone);   
}