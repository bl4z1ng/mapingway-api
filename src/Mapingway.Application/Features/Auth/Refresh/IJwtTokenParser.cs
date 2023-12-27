using System.Security.Claims;

namespace Mapingway.Application.Features.Auth.Refresh;

//TODO: move from Application
public interface IJwtTokenParser
{
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isTokenExpired = false);

    public string? GetEmailFromExpiredToken(string expiredToken, bool isTokenExpired = false);
}
