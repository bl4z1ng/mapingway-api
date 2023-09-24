using Mapingway.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace Mapingway.API.OptionsSetup;

public class DbOptionsSetup : IConfigureOptions<DbOptions>
{
    private readonly IConfiguration _configuration;


    public DbOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void Configure(DbOptions options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var section = _configuration.GetSection(DbOptions.ConfigurationSection);

        section[nameof(DbOptions.ConnectionString)] = connectionString;

        section.Bind(options);
    }
}