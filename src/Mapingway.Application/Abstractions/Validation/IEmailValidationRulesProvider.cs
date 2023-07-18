namespace Mapingway.Application.Abstractions.Validation;

public interface IEmailValidationRulesProvider
{
    bool IsValidEmail(string email);
}