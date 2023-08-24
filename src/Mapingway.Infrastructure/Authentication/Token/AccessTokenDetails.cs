using System.Security.Claims;

namespace Mapingway.Infrastructure.Authentication.Token;

public record AccessTokenDetails(
    string Issuer,
    string Audience,
    TimeSpan TokenLifeSpan,
    byte[] SigningKeyBytes,
    IEnumerable<Claim> Claims)
{
    public virtual bool Equals(AccessTokenDetails? other)
    {
        if (other is null) return false;

        return
            Issuer == other.Issuer &&
            Audience == other.Audience &&
            TokenLifeSpan == other.TokenLifeSpan &&
            SigningKeyBytes.SequenceEqual(other.SigningKeyBytes) &&
            Claims.SequenceEqual(other.Claims);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Issuer, Audience, TokenLifeSpan, SigningKeyBytes, Claims);
    }
}