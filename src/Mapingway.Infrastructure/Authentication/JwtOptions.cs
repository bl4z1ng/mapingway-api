namespace Mapingway.Infrastructure.Authentication;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";

    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string SigningKey { get; init; } = null!;
    public TimeSpan AccessTokenLifetime { get; init; }
    public TimeSpan RefreshTokenLifetime { get; init; }
}