using Mapingway.Application.Contracts.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Security;

public static class Configuration
{
    public static IServiceCollection AddHashing(this IServiceCollection services)
    {
        return services
            .AddValidatedOptions<HashOptions>(HashOptions.ConfigurationSection)
            .AddScoped<IHasher, Hasher>();
    }
}

public class HashOptions
{
    public const string ConfigurationSection = "Hash";

    public string Pepper { get; init; } = null!;
    public int Iterations { get; init; }
}
