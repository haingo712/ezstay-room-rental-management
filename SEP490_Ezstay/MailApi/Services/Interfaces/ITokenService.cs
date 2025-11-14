using System.Security.Claims;

namespace MailApi.Services.Interfaces;
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
        string? GetRoleFromClaims(ClaimsPrincipal user);
    }

