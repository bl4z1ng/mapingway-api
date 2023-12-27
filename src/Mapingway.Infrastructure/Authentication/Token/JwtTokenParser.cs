using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication.Token;

public class JwtTokenParser : IJwtTokenParser
{
    private readonly TokenValidationParameters _expiredTokenValidationParameters;
    private readonly ILogger<JwtTokenParser> _logger;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtTokenParser(ILoggerFactory loggerFactory, IOptions<TokenValidationParameters> options)
    {
        _logger = loggerFactory.CreateLogger<JwtTokenParser>();

        var validationParameters = options.Value;

        _tokenValidationParameters = validationParameters;

        _expiredTokenValidationParameters = validationParameters.Clone();
        _expiredTokenValidationParameters.ValidateLifetime = false;
    }


    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isTokenExpired = false)
    {
        var tokenValidationParameters = isTokenExpired ? _expiredTokenValidationParameters : _tokenValidationParameters;
        var tokenValidationHandler = new JwtSecurityTokenHandler();

        SecurityToken securityToken;
        ClaimsPrincipal principal;
        try
        {
            principal = tokenValidationHandler.ValidateToken(
                token, 
                tokenValidationParameters, 
                out securityToken);
        }
        catch (ArgumentException e)
        {
            _logger.LogError("Recieved access token is invalid: {Token}", token);
            throw new SecurityTokenException(message: "Recieved token is not valid", innerException: e);
        }
        // TODO: two exceptions?
        if (securityToken is not JwtSecurityToken)
        {
            throw new SecurityTokenException("Recieved token is not valid");
        }

        return principal;
    }
}