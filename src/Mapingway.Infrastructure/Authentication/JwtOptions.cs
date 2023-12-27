using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mapingway.Infrastructure.Authentication;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";

    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigningKey { get; init; }
    public string UserContextSalt { get; init; } = null!;

    [BindRequired]
    public TimeSpan AccessTokenLifetime { get; init; }

    [BindRequired]
    public TimeSpan RefreshTokenLifetime { get; init; }
}
