using Mapingway.Application.Contracts.Authentication;
using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.Infrastructure.Authentication.Token.Parser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IHasher _hasher;
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtBearerOptionsSetup(
        IOptions<TokenValidationParameters> tokenValidationParameters,
        IHasher hasher,
        IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
        _hasher = hasher;
        _tokenValidationParameters = tokenValidationParameters.Value;
    }

    // https://stackoverflow.com/questions/71132926/jwtbeareroptions-configure-method-not-getting-executed
    // The subtility here is that AddJwtBearer() uses a named options delegate.
    // Instead of implementing IConfigureOptions, need to implement IConfigureNamedOptions
    public void Configure(string? name, JwtBearerOptions options)
    {
        //to not use Microsoft claims naming
        options.MapInboundClaims = false;
        options.TokenValidationParameters = _tokenValidationParameters;
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                //TODO: move from here
                var userContextCookieMissing =
                    !context.Request.Cookies.TryGetValue(CustomClaims.UserContext, out var userContextCookie);
                var userContextHashClaimMissing =
                    !context.Principal!.HasClaim(c => c.Type == CustomClaims.UserContext);

                if (userContextCookieMissing || userContextHashClaimMissing)
                {
                    context.Fail("User context is missing or removed. Try to log-in again.");

                    return Task.CompletedTask;
                }

                var userContextHashClaim = context.Principal.GetUserContextClaim();
                var userContextHashCookie = _hasher.GenerateHash(userContextCookie!, _jwtOptions.UserContextSalt);

                if (userContextHashClaim != userContextHashCookie)
                {
                    context.Fail("User context is invalid. Try to log-in again.");
                }

                return Task.CompletedTask;
            }
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
