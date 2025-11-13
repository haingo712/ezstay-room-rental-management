using System.Security.Claims;

namespace ChatAPI.Service.Interface
{
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
       
    }
}
