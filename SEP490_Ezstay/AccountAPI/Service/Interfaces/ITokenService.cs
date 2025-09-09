using System.Security.Claims;

namespace AccountAPI.Service.Interfaces
{
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
        string? GetFullNameFromClaims(ClaimsPrincipal user);
        string? GetPhoneFromClaims(ClaimsPrincipal user);
    }
}
