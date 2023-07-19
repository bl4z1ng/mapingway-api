using Mapingway.Infrastructure.Validation.Email;
using Microsoft.Extensions.Options;

namespace Mapingway.API.OptionsSetup.Validation;

public class EmailValidationRulesSetup : IConfigureOptions<EmailValidationRules>
{
    private readonly IConfiguration _configuration;


    public EmailValidationRulesSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void Configure(EmailValidationRules options)
    {
        var path = $"{ValidationOptions.ConfigurationSection}:{EmailValidationRules.ConfigurationSection}";

        _configuration.GetSection(path).Bind(options);
    }
}