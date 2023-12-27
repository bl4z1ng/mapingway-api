using System.IdentityModel.Tokens.Jwt;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Features.Auth.Refresh;
using Mapingway.Infrastructure.Authentication.Permissions;
using Mapingway.Infrastructure.Authentication.Token;
using Mapingway.Infrastructure.Authentication.Token.Parser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Authentication;

public static class Configuration
{
    //TODO: cleanup
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureJwt(configuration)
            .AddAuthenticationServices()
            .AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

        services
            .AddAuthorization()
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    private static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        // to not use Microsoft claims naming
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddOptionsWithValidateOnStart<JwtOptions>()
            .BindConfiguration(JwtOptions.ConfigurationSection);

        services.ConfigureOptions<TokenValidationParametersSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddTransient<IJwtTokenParser, JwtTokenParser>();

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        return services;
    }
}
