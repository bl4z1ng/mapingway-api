using System.IdentityModel.Tokens.Jwt;
using Mapingway.API.OptionsSetup;
using Mapingway.Infrastructure.Authentication;

namespace Mapingway.API.Extensions.Configuration;

public static class JwtConfiguration
{
    public static WebApplicationBuilder ConfigureJwt(this WebApplicationBuilder builder)
    {
        // to not use Microsoft claims naming
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        var services = builder.Services;
        var configuration = builder.Configuration;

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.ConfigurationSection))
            .ValidateOnStart();
        
        services.ConfigureOptions<TokenValidationParametersSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddTransient<IJwtTokenParser, JwtTokenParser>();

        return builder;
    }
}