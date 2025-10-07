using ContractAPI.DTO.Response;

namespace ContractAPI.APIs.Interfaces;

public interface IAccountAPI
{
    Task<IdentityProfileResponseDto?> GetProfileByPhoneAsync(string phone);   
}