using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication;

public interface IJwtTokenParser
{
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isTokenExpired = false);
}