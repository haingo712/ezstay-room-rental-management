 
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using ChatAPI.Service.Interface;

    namespace ChatAPI.Service
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
    }
}
