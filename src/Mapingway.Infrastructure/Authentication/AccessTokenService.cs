using Mapingway.Application.Abstractions.Authentication;

namespace Mapingway.Infrastructure.Authentication;

public class AccessTokenService : IAccessTokenService
{
    private readonly IJwtTokenParser _jwtTokenParser;


    public AccessTokenService(IJwtTokenParser jwtTokenParser)
    {
        _jwtTokenParser = jwtTokenParser;
    }


    public string? GetEmailFromExpiredToken(string expiredToken)
    {
        var principal = _jwtTokenParser.GetPrincipalFromToken(expiredToken, true);

        return principal.GetEmailClaim();
    }
}