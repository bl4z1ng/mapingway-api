using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public interface IJwtTokenParser
{
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isTokenExpired = false);
}