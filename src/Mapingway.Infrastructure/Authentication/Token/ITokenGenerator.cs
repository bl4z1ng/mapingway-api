using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public interface ITokenGenerator
{
    string? GenerateAccessToken(
        string issuer,
        string audience,
        TimeSpan tokenLifespan,
        byte[] signingKey,
        IEnumerable<Claim> claims);

    string GenerateRefreshToken();
}