using Mapingway.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure;

public class DevelopmentDbContext : ApplicationDbContext
{
    public DevelopmentDbContext(IOptions<DatabaseConfigurationOptions> configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.ConnectionString);
    }
}