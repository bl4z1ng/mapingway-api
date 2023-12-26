using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RefreshToken> Tokens { get; set; } = null!;
    public DbSet<RefreshTokenFamily> ExpiredTokenFamilies { get; set; } = null!;


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
        optionsBuilder.UseNpgsql(Configuration.ConnectionString);

        optionsBuilder.EnableSensitiveDataLogging(Configuration.EnableSensitiveDataLogging);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}