using System.Text.RegularExpressions;
using Mapingway.Application.Abstractions.Validation;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Validation.Email;

public class EmailValidationRulesProvider : IEmailValidationRulesProvider
{
    private readonly EmailValidationRules _emailRules;


    public EmailValidationRulesProvider(IOptions<EmailValidationRules> emailRules)
    {
        _emailRules = emailRules.Value;
    }

    // TODO: Add IsEmailUnique validation
    public bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, _emailRules.Pattern);
    }
}