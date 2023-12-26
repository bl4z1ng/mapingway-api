namespace Mapingway.Application.Contracts.Validation;

public interface IPasswordValidationRulesProvider
{
    int NumberOfLetters { get; }
    bool HasNOrMoreLetters(string str);
}