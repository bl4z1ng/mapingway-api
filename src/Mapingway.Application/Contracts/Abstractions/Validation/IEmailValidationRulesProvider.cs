namespace Mapingway.Application.Contracts.Abstractions.Validation;

public interface IEmailValidationRulesProvider
{
    bool IsEmailValid(string email);
    public Task<bool> IsEmailUnique(string email);
}