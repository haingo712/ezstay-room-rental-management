using System.Security.Claims;
using APIGateway.Helper.Interfaces;

namespace APIGateway.Helper
{
    public class UserClaimHelper : IUserClaimHelper
    {
        public Guid GetUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claim))
                throw new UnauthorizedAccessException("Không xác định được UserId từ token.");
            return Guid.Parse(claim);
        }

        public string? GetFullName(ClaimsPrincipal user)
        {
            return user.FindFirst("fullName")?.Value;
        }

        public string? GetPhone(ClaimsPrincipal user)
        {
            return user.FindFirst("phone")?.Value;
        }
    }
}
