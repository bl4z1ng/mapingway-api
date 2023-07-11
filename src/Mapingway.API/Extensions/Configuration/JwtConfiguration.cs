using Mapingway.API.OptionsSetup;

namespace Mapingway.API.Extensions.Configuration;

public static class JwtConfiguration
{
    public static IServiceCollection ConfigureJwt(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<TokenValidationParametersSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        return services;
    }
}