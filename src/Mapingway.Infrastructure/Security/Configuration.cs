using Mapingway.Application.Contracts.Authentication;
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

        //TODO: change to scoped, if access state
        services.AddTransient<IHasher, Hasher>();

        return services;
    }
}
