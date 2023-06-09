using Mapingway.Common.Options;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<User>? Users { get; set; } = null;

    protected readonly DatabaseConfigurationOptions Configuration;
    
    public ApplicationDbContext(IOptions<DatabaseConfigurationOptions> configuration)
    {
        Configuration = configuration.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration?.ConnectionString);
        
        optionsBuilder.EnableSensitiveDataLogging(Configuration?.EnableSensitiveDataLogging ?? false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}