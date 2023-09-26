using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Authentication.Token;

namespace Mapingway.API.Installers;

public static class AuthenticationServiceInstaller
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}