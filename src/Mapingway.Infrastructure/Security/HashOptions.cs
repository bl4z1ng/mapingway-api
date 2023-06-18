namespace Mapingway.Infrastructure.Security;

public class HashOptions
{
    public const string ConfigurationSection = "Hash";

    public string Pepper { get; init; } = null!;
    public int Iterations { get; init; }
}