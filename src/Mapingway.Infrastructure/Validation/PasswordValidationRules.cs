using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Infrastructure.Validation;

public class PasswordValidationRules : IPasswordValidationRules
{
    public const string ConfigurationSection = "Password";

    public string NOrMoreLettersPattern { get; init; } = null!;
    public int NumberOfLetters { get; init; }
}