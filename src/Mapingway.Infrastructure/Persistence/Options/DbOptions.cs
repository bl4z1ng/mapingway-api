namespace Mapingway.Infrastructure.Persistence.Options;

public class DbOptions
{
    public string? ConnectionString { get; init; }
    public string? DefaultScheme { get; init; }
    public bool EnableLogging { get; init; }
    public bool EnableSensitiveDataLogging { get; init; }
    public bool EnableDetailedErrors { get; init; }
    
    public const string DevelopmentConfigurationSection = "DevelopmentDatabase";
    public const string ProductionConfigurationSection = "ProductionDatabase";
}