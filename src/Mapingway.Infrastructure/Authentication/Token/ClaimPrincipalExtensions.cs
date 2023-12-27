using System.Security.Claims;
using Mapingway.Infrastructure.Authentication.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public static class ClaimPrincipalExtensions
{
    public static string? GetUserContextClaim(this ClaimsPrincipal user)
    {
        return user.Claims.
            FirstOrDefault(claim => claim.Type == CustomClaims.UserContext)?.Value;
    }
}
