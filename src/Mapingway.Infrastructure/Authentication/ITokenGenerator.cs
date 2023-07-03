using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication;

public interface ITokenGenerator
{
    string GenerateAccessToken(
        string issuer,
        string audience,
        TimeSpan tokenLifespan,
        byte[] signingKey,
        IEnumerable<Claim> claims);

    string GenerateRefreshToken();
}