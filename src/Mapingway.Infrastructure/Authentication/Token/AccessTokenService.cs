using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Infrastructure.Authentication.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Authentication.Token;

public class AccessTokenService : IAccessTokenService
{
    private readonly IHasher _hasher;
    private readonly JwtOptions _jwtOptions;
    private readonly IJwtTokenParser _jwtTokenParser;
    private readonly IPermissionRepository _permissions;
    private readonly ITokenGenerator _tokenGenerator;


    public AccessTokenService(
        ILoggerFactory loggerFactory,
        IOptions<JwtOptions> jwtOptions, 
        ITokenGenerator tokenGenerator,
        IHasher hasher,
        IUnitOfWork unitOfWork, IJwtTokenParser jwtTokenParser)
    {
        loggerFactory.CreateLogger<AccessTokenService>();

        _jwtOptions = jwtOptions.Value;
        _jwtTokenParser = jwtTokenParser;
        _tokenGenerator = tokenGenerator;
        _hasher = hasher;

        _permissions = unitOfWork.Permissions;
    }


    public async Task<AccessUnit> GenerateAccessToken(long userId, string email, CancellationToken? ct = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email)
        };

        var permissions = await _permissions.GetPermissionsAsync(userId, ct ?? CancellationToken.None);
        claims.AddRange(permissions.Select(p => new Claim(CustomClaims.Permissions, p)));
        
        var userContextToken = _tokenGenerator.GenerateRandomToken();
        var userContextHash = _hasher.GenerateHash(userContextToken, _jwtOptions.UserContextSalt);
        claims.Add(new Claim(CustomClaims.UserContext, userContextHash));

        var signingKey = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
        var details = new AccessTokenDetails(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            _jwtOptions.AccessTokenLifetime,
            signingKey,
            claims);

        var token = _tokenGenerator.GenerateAccessToken(details);

        return new AccessUnit
        {
            AccessToken = token,
            UserContextToken = userContextToken
        };
    }

    public string? GetEmailFromExpiredToken(string expiredToken)
    {
        var principal = _jwtTokenParser.GetPrincipalFromToken(expiredToken, true);

        return principal.GetEmailClaim();
    }
}