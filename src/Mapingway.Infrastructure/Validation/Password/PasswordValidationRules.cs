namespace Mapingway.Infrastructure.Validation.Password;

public class PasswordValidationRules
{
    public const string ConfigurationSection = "Password";

    public string NOrMoreLettersPattern { get; init; } = null!;
    public int NumberOfLetters { get; init; }
}