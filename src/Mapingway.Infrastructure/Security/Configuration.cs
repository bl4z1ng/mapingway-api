using Mapingway.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Security;

public static class Configuration
{
    public static IServiceCollection AddHashing(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<HashOptions>()
            .Bind(configuration.GetSection(HashOptions.ConfigurationSection))
            .ValidateOnStart();

        services.AddTransient<IHasher, Hasher>();

        return services;
    }
}
