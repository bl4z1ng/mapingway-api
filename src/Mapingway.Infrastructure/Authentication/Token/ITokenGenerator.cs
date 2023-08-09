using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public interface ITokenGenerator
{
    string? GenerateAccessToken(AccessTokenDetails details);

    string GenerateRefreshToken();
}