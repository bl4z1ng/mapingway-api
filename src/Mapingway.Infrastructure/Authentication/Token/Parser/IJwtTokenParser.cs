using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token.Parser;

public interface IJwtTokenParser
{
    public ClaimsPrincipal GetPrincipalFromBearer(string token, bool tokenExpired = false);
}
