using System.IdentityModel.Tokens.Jwt;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Infrastructure.Authentication.Permissions;
using Mapingway.Infrastructure.Authentication.Token;
using Mapingway.Infrastructure.Authentication.Token.Parser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Authentication;

public static class Configuration
{
    //TODO: cleanup
    internal static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .ConfigureJwt()
            .AddAuthServices()
            .AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

        services
            .AddAuthorization()
            .AddPermissionHandling();

        return services;
    }

    private static IServiceCollection ConfigureJwt(this IServiceCollection services)
    {
        // to not use Microsoft claims naming
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddOptionsWithValidateOnStart<JwtOptions>().BindConfiguration(JwtOptions.ConfigurationSection);
        services.ConfigureOptions<TokenValidationParametersSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IJwtTokenParser, JwtTokenParser>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        return services;
    }

    private static IServiceCollection AddPermissionHandling(this IServiceCollection services)
    {
        return services
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
}
