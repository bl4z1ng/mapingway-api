using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mapingway.Common.Constants;

namespace Mapingway.API.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string? GetEmailClaim(this ClaimsPrincipal user)
    {
        return user.Claims
            .FirstOrDefault(claim => claim.Type == WsDecodedClaimTypes.Keys[JwtRegisteredClaimNames.Email])?.Value;
    }
    
    public static string? GetUserContextTokenClaim(this ClaimsPrincipal user)
    {
        return user.Claims.
            FirstOrDefault(claim => claim.Type == CustomClaimNames.UserContext)?.Value;
    }
}