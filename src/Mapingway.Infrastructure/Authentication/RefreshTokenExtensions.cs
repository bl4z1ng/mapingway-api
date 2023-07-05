using Mapingway.Domain;
using Mapingway.Domain.Auth;

namespace Mapingway.Infrastructure.Authentication;

public static class RefreshTokenExtensions
{
    public static RefreshToken CreateNotUsed(string value, TimeSpan lifetime)
    {
        return new RefreshToken
        {
            Value = value,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.Add(lifetime)
        };
    }
}