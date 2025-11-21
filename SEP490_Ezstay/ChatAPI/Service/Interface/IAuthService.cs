using Shared.DTOs.Auths.Responses;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service.Interface;

public interface IAuthService
{ 
    Task<ChatUserInfoResponse> GetById(Guid accountId);
    
}   