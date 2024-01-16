namespace Mapingway.Domain.Auth;

public class RefreshTokenFamily
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public User User { get; init; } = null!;
    public ICollection<RefreshToken> Tokens { get; init; } = new List<RefreshToken>();

    public bool InvalidateToken(string refreshTokenKey)
    {
        var refreshToken = Tokens.FirstOrDefault(token => token.Value == refreshTokenKey);
        if (refreshToken is null) return false;

        refreshToken.IsUsed = true;

        return true;
    }

    public bool InvalidateAllActiveTokens()
    {
        var notUsedTokens = Tokens.Where(token => !token.IsUsed).ToList();

        var isTokenInvalidatedFlags =
            notUsedTokens.Select(token => InvalidateToken(token.Value));

        var isErrorOccurred = isTokenInvalidatedFlags.Any(invalidated => !invalidated);

        return isErrorOccurred;
    }
}
