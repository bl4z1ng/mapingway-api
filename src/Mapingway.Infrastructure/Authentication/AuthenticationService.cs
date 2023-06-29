using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Common.Constants;
using Mapingway.Domain.Auth;
using Mapingway.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationService(
        IOptions<JwtOptions> jwtOptions, 
        IOptions<TokenValidationParameters> tokenValidationOptions, 
        IUnitOfWork unitOfWork)
    {
        _jwtOptions = jwtOptions.Value;

        _tokenValidationParameters = tokenValidationOptions.Value;
        _tokenValidationParameters.ValidateLifetime = false;

        _refreshTokenRepository = unitOfWork.RefreshTokens;
    }


    public string GenerateAccessToken(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        claims.AddRange(permissions.Select(p => new Claim(CustomClaimNames.Permissions, p)));

        var signingKey = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)), 
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(_jwtOptions.AccessTokenLifetime),
            signingKey);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }

    public string GenerateRefreshToken()
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        rng.GetBytes(bytes);

        var token = Convert.ToBase64String(bytes);

        return token;
    }

    public async Task BindRefreshTokenToUserAsync(User user, string refreshToken, CancellationToken? cancellationToken = null)
    {
        var refreshTokenEntity = new RefreshToken
        {
            Value = refreshToken,
            User = user,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenLifetime)
        };

        await _refreshTokenRepository.CreateAsync(refreshTokenEntity, cancellationToken ?? CancellationToken.None);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationHandler = new JwtSecurityTokenHandler();

        var principal = tokenValidationHandler.ValidateToken(
            expiredToken, 
            _tokenValidationParameters, 
            out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}