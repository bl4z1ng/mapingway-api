using Mapingway.Domain.Auth;

namespace Mapingway.Infrastructure.Authentication;

public static class RefreshTokenExtensions
{
    public static RefreshToken CreateNotUsed(string value, int userId, TimeSpan expiresAt)
    {
        return new RefreshToken
        {
            Value = value,
            IsUsed = false,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.Add(expiresAt)
        };
    }
}