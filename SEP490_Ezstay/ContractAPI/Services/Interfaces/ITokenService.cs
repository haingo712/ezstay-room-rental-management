using System.Security.Claims;

namespace ContractAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
        string? GetFullNameFromClaims(ClaimsPrincipal user);
        string? GetPhoneFromClaims(ClaimsPrincipal user);
    }
}
