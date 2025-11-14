using Shared.DTOs.Auths.Responses;

namespace ChatAPI.Service.Interface;

public interface IAuthService
{ 
    Task<AccountResponse?> GetById(Guid accountId);
    
}   