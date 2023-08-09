using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public record AccessTokenDetails(
    string Issuer,
    string Audience,
    TimeSpan TokenLifeSpan,
    byte[] SigningKeyBytes,
    IEnumerable<Claim> Claims);