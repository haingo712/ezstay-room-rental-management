using APIGateway.Helper.Interfaces;
using System.Security.Claims;
using UserManagerAPI.Service.Interfaces;

namespace UserManagerAPI.Service
{
    public class JwtClaimHelper : IJwtClaimHelper
    {
        public Guid GetUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claim))
                throw new UnauthorizedAccessException("Không xác định được UserId từ token.");
            return Guid.Parse(claim);
        }

        public string? GetFullName(ClaimsPrincipal user) =>
            user.FindFirst("fullName")?.Value;

        public string? GetPhone(ClaimsPrincipal user) =>
            user.FindFirst("phone")?.Value;

        public string? GetEmail(ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.Email)?.Value ?? user.FindFirst("email")?.Value;

        public Guid? GetUserIdOrNull(ClaimsPrincipal user)
        {
            // Lấy từ NameIdentifier trước, nếu không có lấy từ sub (JWT standard)
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(claim))
                return null;

            // Nếu claim là Guid thì parse, nếu là email/sub thì trả null
            return Guid.TryParse(claim, out var id) ? id : null;
        }

    }

}
