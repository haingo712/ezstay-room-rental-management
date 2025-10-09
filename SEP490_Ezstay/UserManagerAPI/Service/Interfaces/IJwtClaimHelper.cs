using System.Security.Claims;

namespace UserManagerAPI.Service.Interfaces
{
    public interface IJwtClaimHelper
    {
        Guid GetUserId(ClaimsPrincipal user);
        string? GetFullName(ClaimsPrincipal user);
        string? GetPhone(ClaimsPrincipal user);
        string? GetEmail(ClaimsPrincipal user);
        Guid? GetUserIdOrNull(ClaimsPrincipal user);
    }
}
