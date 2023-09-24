using Mapingway.Application.Abstractions;
using Mapingway.Infrastructure.Security;

namespace Mapingway.API.Configurations;

public static class HashingConfiguration
{
    public static WebApplicationBuilder ConfigureHashing(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services
            .AddOptions<HashOptions>()
            .Bind(configuration.GetSection(HashOptions.ConfigurationSection))
            .ValidateOnStart();

        services.AddTransient<IHasher, Hasher>();

        return builder;
    }
}