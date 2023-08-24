namespace Mapingway.Infrastructure.Validation.Password;

public class PasswordValidationRules : IValidationRules
{
    public static string ConfigurationSection => "Password";

    public string NOrMoreLettersPattern { get; init; } = null!;
    public int NumberOfLetters { get; init; }
}