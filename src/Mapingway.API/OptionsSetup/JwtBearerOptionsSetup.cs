using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.API.OptionsSetup;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtBearerOptionsSetup(IOptions<TokenValidationParameters> tokenValidationParameters)
    {
        _tokenValidationParameters = tokenValidationParameters.Value;
    }


    // https://stackoverflow.com/questions/71132926/jwtbeareroptions-configure-method-not-getting-executed
    // The subtility here is that AddJwtBearer() uses a named options delegate.
    // Instead of implementing IConfigureOptions, need to implement IConfigureNamedOptions
    public void Configure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters = _tokenValidationParameters;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}