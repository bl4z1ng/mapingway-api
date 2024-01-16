using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Claims.Parser;

public interface IJwtTokenParser
{
    public ClaimsPrincipal GetPrincipalFromBearer(string token);
}
