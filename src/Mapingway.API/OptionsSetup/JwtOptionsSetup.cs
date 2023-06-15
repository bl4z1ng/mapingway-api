using Mapingway.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Mapingway.API.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(JwtOptions.ConfigurationSection).Bind(options);
    }
}