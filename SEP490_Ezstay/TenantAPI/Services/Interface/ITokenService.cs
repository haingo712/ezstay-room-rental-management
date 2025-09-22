using System.Security.Claims;

namespace TenantAPI.Services.Interface
{
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
        string? GetFullNameFromClaims(ClaimsPrincipal user);
        string? GetPhoneFromClaims(ClaimsPrincipal user);
    }
}
