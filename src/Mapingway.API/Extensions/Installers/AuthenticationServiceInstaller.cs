using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Authentication.Token;

namespace Mapingway.API.Extensions.Installers;

public static class AuthenticationServiceInstaller
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}