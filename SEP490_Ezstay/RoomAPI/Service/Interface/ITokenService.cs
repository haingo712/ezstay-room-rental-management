﻿using System.Security.Claims;

namespace RoomAPI.Service.Interface
{
    public interface ITokenService
    {
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
        string? GetFullNameFromClaims(ClaimsPrincipal user);
        string? GetPhoneFromClaims(ClaimsPrincipal user);
    }
}
