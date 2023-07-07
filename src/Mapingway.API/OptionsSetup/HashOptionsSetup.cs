using Mapingway.Infrastructure.Security;
using Microsoft.Extensions.Options;

namespace Mapingway.API.OptionsSetup;

public class HashOptionsSetup : IConfigureOptions<HashOptions>
{
    private readonly IConfiguration _configuration;


    public HashOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void Configure(HashOptions options)
    {
        _configuration.GetSection(HashOptions.ConfigurationSection).Bind(options);
    }
}