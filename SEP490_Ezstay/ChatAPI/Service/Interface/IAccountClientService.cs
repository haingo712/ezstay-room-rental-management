using Shared.DTOs.Accounts.Responses;

namespace ChatAPI.Service.Interface;

public interface IAccountClientService
{
    Task<AccountResponse?> GetByIdAsync(Guid postId);
}