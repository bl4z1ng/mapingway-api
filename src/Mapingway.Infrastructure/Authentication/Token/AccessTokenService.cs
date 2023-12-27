using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Errors;
using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.SharedKernel.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Authentication.Token;

public class AccessTokenService : IAccessTokenService
{
    private readonly IHasher _hasher;
    private readonly JwtOptions _jwtOptions;
    private readonly IPermissionRepository _permissions;
    private readonly ITokenGenerator _tokenGenerator;

    public AccessTokenService(
        ILoggerFactory loggerFactory,
        IOptions<JwtOptions> jwtOptions,
        ITokenGenerator tokenGenerator,
        IHasher hasher,
        IUnitOfWork unitOfWork)
    {
        _jwtOptions = jwtOptions.Value;
        _tokenGenerator = tokenGenerator;
        _hasher = hasher;

        _permissions = unitOfWork.Permissions;

        loggerFactory.CreateLogger<AccessTokenService>();
    }

    public async Task<Result<AccessUnit>> GenerateAccessToken(long userId, string email, CancellationToken ct = default)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email)
        };

        var permissions = await _permissions.GetPermissionsAsync(userId, ct);
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
        if (token is null) return Result.Failure<AccessUnit>(TokenError.FailedToGenerate);

        return new AccessUnit
        {
            AccessToken = token,
            UserContextToken = userContextToken
        };
    }
}
