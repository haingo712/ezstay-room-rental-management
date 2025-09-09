 
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using RoomAPI.Service.Interface;

    namespace RoomAPI.Service
    {
        public class TokenService : ITokenService
        {
        public Guid GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claim))
                throw new UnauthorizedAccessException("Không xác định được UserId từ token.");

            return Guid.Parse(claim);
        }

        public string? GetFullNameFromClaims(ClaimsPrincipal user)
        {
            return user.FindFirst("fullName")?.Value;
        }

        public string? GetPhoneFromClaims(ClaimsPrincipal user)
        {
            return user.FindFirst("phone")?.Value;
        }

    }
}
