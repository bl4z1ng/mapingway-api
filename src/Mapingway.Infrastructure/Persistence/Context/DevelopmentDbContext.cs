using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Persistence.Context;

public class DevelopmentDbContext : ApplicationDbContext
{
    public DevelopmentDbContext(DbContextOptions<DevelopmentDbContext> options, IOptions<DbOptions> configuration) 
        : base(options, configuration)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.ConnectionString);

        optionsBuilder.EnableSensitiveDataLogging(Configuration.EnableSensitiveDataLogging);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevelopmentDbContext).Assembly);
    }
}