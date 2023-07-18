namespace Mapingway.Application.Abstractions;

public interface IPasswordValidationRulesProvider
{
    int PasswordNumberOfLetters { get; }
    bool HasNOrMoreLetters(string str);
}

public interface IValidationRulesProvider : IPasswordValidationRulesProvider
{
    bool IsValidEmail(string email);
}