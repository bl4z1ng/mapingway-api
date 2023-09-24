using System.Security.Claims;
using Mapingway.Infrastructure.Authentication.Claims;

namespace Mapingway.API.Internal.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string? GetUserContextTokenClaim(this ClaimsPrincipal user)
    {
        return user.Claims.
            FirstOrDefault(claim => claim.Type == CustomClaimNames.UserContext)?.Value;
    }
}