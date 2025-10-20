using Shared.DTOs.Auths.Responses;

namespace ChatAPI.Service.Interface;

public interface IAccountClientService
{ 
    Task<AccountResponse?> GetByIdAsync(Guid postId);
    
}