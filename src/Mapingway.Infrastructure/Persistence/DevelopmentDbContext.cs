using Mapingway.Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Persistence;

public class DevelopmentDbContext : ApplicationDbContext
{
    public DevelopmentDbContext(DbContextOptions<DevelopmentDbContext> options, IOptions<DbOptions> configuration) : base(options, configuration)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.ConnectionString);

        optionsBuilder.EnableSensitiveDataLogging(Configuration?.EnableSensitiveDataLogging ?? false);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevelopmentDbContext).Assembly);
    }
}