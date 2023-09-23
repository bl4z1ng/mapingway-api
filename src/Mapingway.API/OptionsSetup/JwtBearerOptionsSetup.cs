using Mapingway.API.Extensions;
using Mapingway.Application.Abstractions;
using Mapingway.Common.Constants;
using Mapingway.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.API.OptionsSetup;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IHasher _hasher;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtOptions _jwtOptions;


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
        options.TokenValidationParameters = _tokenValidationParameters;
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var userContextCookieMissing =
                    !context.Request.Cookies.TryGetValue(CustomClaimNames.UserContext, out var userContextCookie);
                var userContextHashClaimMissing =
                    !context.Principal!.HasClaim(c => c.Type == CustomClaimNames.UserContext);

                if (userContextCookieMissing || userContextHashClaimMissing)
                {
                    context.Fail("User context is missing or removed. Try to log-in again");

                    return Task.CompletedTask;
                }

                var userContextHashClaim = context.Principal.GetUserContextTokenClaim();
                var userContextHashCookie = _hasher.GenerateHash(userContextCookie!, _jwtOptions.UserContextSalt);

                if (userContextHashClaim != userContextHashCookie)
                {
                    context.Fail("User context is invalid. Try to log-in again");
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