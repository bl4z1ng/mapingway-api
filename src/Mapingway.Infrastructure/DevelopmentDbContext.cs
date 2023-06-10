using Mapingway.Common.Options;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure;

public class DevelopmentDbContext : ApplicationDbContext
{
    // public DbSet<User>? Users { get; set; }

    // private readonly DatabaseConfiguration _configuration;
    
    public DevelopmentDbContext(DbContextOptions<DevelopmentDbContext> options, IOptions<DbOptions> configuration) : base(options, configuration)
    {
        // _configuration = configuration.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite(_configuration.ConnectionString);
        optionsBuilder.UseSqlite(Configuration.ConnectionString);

        //optionsBuilder.EnableSensitiveDataLogging(_configuration?.EnableSensitiveDataLogging ?? false);
        optionsBuilder.EnableSensitiveDataLogging(Configuration?.EnableSensitiveDataLogging ?? false);

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevelopmentDbContext).Assembly);
    }
}