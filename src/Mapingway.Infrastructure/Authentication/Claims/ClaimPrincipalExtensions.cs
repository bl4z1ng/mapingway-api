using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Claims;

public static class ClaimPrincipalExtensions
{
    public static string? GetEmailClaim(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value;
    }

    public static string? GetUserContextClaim(this ClaimsPrincipal user)
    {
        return user.Claims.
            FirstOrDefault(claim => claim.Type == CustomClaims.UserContext)?.Value;
    }
}
