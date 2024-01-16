using Mapingway.Application.Contracts.Authentication;
using Mapingway.Infrastructure.Authentication.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Authentication;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private const string _userContextIsInvalidMessage = "User context is invalid or missing. Try to log-in again.";
    private readonly JwtOptions _jwtOptions;

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    // https://stackoverflow.com/questions/71132926/jwtbeareroptions-configure-method-not-getting-executed
    // The subtility here is that AddJwtBearer() uses a named options delegate, so
    // instead of implementing IConfigureOptions, need to implement IConfigureNamedOptions.
    public void Configure(JwtBearerOptions options) => Configure(JwtBearerDefaults.AuthenticationScheme, options);

    public void Configure(string? name, JwtBearerOptions options)
    {
        //to not use Microsoft claims naming
        options.MapInboundClaims = false;

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var cookieMissing = !context.Request.Cookies.TryGetValue(CustomClaims.UserContext, out var cookie);
                var hashClaim = context.Principal!.GetUserContextClaim();

                if (cookieMissing || hashClaim is null)
                {
                    context.Fail(_userContextIsInvalidMessage);

                    return Task.CompletedTask;
                }

                var hasher = context.HttpContext.RequestServices.GetRequiredService<IHasher>();
                var cookieHash = hasher.GenerateHash(cookie!, _jwtOptions.UserContextSigningKey);

                if (hashClaim != cookieHash) context.Fail(_userContextIsInvalidMessage);

                return Task.CompletedTask;
            }
        };
    }
}
