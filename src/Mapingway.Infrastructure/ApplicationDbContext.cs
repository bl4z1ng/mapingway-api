using Mapingway.Common.Options;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    protected readonly DbOptions Configuration;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<DbOptions> configuration):
        base(options)
    {
        Configuration = configuration.Value;
    }

    protected ApplicationDbContext(DbContextOptions options, IOptions<DbOptions> configuration):
        base(options)
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