using System.Security.Claims;

namespace APIGateway.Helper.Interfaces
{
    public interface IUserClaimHelper
    {
        Guid GetUserId(ClaimsPrincipal user);
        string? GetFullName(ClaimsPrincipal user);
        string? GetPhone(ClaimsPrincipal user);
        string? GetEmail(ClaimsPrincipal user);
    }
}
