using System.Text.RegularExpressions;
using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Application.Contracts.Abstractions.Validation;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Validation.Email;

public class EmailValidationRulesProvider : IEmailValidationRulesProvider
{
    private readonly IUserRepository _userRepository;
    private readonly EmailValidationRules _emailRules;


    public EmailValidationRulesProvider(IOptions<EmailValidationRules> emailRules, IUserRepository userRepository)
    {
        _emailRules = emailRules.Value;
        _userRepository = userRepository;
    }

    public bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, _emailRules.Pattern);
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email, CancellationToken.None);

        return user is null;
    }
}