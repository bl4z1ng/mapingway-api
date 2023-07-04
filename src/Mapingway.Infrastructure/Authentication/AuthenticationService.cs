using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Common.Constants;
using Mapingway.Common.Exceptions;
using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationService(
        ILoggerFactory loggerFactory,
        IOptions<JwtOptions> jwtOptions, 
        IOptions<TokenValidationParameters> tokenValidationOptions, 
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _logger = loggerFactory.CreateLogger(typeof(AuthenticationService));
        _tokenGenerator = tokenGenerator;
        _jwtOptions = jwtOptions.Value;

        _tokenValidationParameters = tokenValidationOptions.Value;
        _tokenValidationParameters.ValidateLifetime = false;

        _unitOfWork = unitOfWork;
        _refreshTokenRepository = unitOfWork.RefreshTokens;
    }


    public string GenerateAccessToken(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        claims.AddRange(permissions.Select(p => new Claim(CustomClaimName.Permissions, p)));

        var signingKey = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);

        var token = _tokenGenerator.GenerateAccessToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            _jwtOptions.AccessTokenLifetime,
            signingKey,
            claims);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.GenerateRefreshToken();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationHandler = new JwtSecurityTokenHandler();

        var principal = tokenValidationHandler.ValidateToken(
            expiredToken, 
            _tokenValidationParameters, 
            out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<RefreshToken?> RefreshTokenAsync(
        User user, 
        string newToken, 
        string? oldToken = null, 
        CancellationToken? cancellationToken = null)
    {
        // TODO: re-do validation.
        if (
            oldToken is null || 
            user.RefreshToken is null || 
            user.RefreshToken.Value == oldToken || 
            user.RefreshToken.ExpiresAt < DateTime.UtcNow)
        {
            var newRefreshToken = await UpdateRefreshTokenAsync(
                user, 
                newToken, 
                cancellationToken ?? CancellationToken.None);

            return newRefreshToken;
        }

        var tokenAlreadyUsed = user.UsedRefreshTokensFamily.Tokens.Any(token => token.Value == newToken);
        if (tokenAlreadyUsed)
        {
            InvalidateRefreshToken(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            _logger.LogWarning("An attempt to use token, that is already used");
            
            throw new RefreshTokenUsedException($"Token {newToken} is already used");
        }

        return null;
    }


    private async Task<RefreshToken?> UpdateRefreshTokenAsync(User user, string newToken, CancellationToken cancellationToken)
    {
        InvalidateRefreshToken(user);

        var refreshToken = RefreshTokenExtensions.CreateNotUsed(
            newToken,
            user.Id,
            _jwtOptions.RefreshTokenLifetime);
        user.RefreshToken = refreshToken;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    private bool InvalidateRefreshToken(User user)
    {
        if (user.RefreshToken is null)
        {
            return false;
        }
        //TODO: ?????????????? Unique constraint failed
        user.RefreshToken.IsUsed = true;
        user.UsedRefreshTokensFamily.Tokens.Add(user.RefreshToken);

        _refreshTokenRepository.Update(user.RefreshToken);

        return true;
    }
}