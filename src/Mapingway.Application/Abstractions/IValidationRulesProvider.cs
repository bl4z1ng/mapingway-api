namespace Mapingway.Application.Abstractions;

public interface IValidationRulesProvider
{
    bool IsValidEmail(string email);
    bool HasNOrMoreLetters(string str);
}