using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Common.Constants;
using Mapingway.Common.Exceptions;
using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger _logger;
    private readonly JwtOptions _jwtOptions;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly TokenValidationParameters _expiredTokenValidationParameters;


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
        
        var tokenValidationParameters = tokenValidationOptions.Value;
        _expiredTokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = tokenValidationParameters.ValidateIssuer,
            ValidateAudience = tokenValidationParameters.ValidateAudience,
            ValidateIssuerSigningKey = tokenValidationParameters.ValidateIssuerSigningKey,
            ValidateLifetime = false,

            ValidIssuer = tokenValidationParameters.ValidIssuer,
            ValidAudience = tokenValidationParameters.ValidAudience,
            ValidAlgorithms = tokenValidationParameters.ValidAlgorithms,
            IssuerSigningKey = tokenValidationParameters.IssuerSigningKey,

            ClockSkew = tokenValidationParameters.ClockSkew
        };

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
            _expiredTokenValidationParameters, 
            out var securityToken);

        if (securityToken is not JwtSecurityToken)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public string? GetEmailFromExpiredToken(string expiredToken)
    {
        var principal = GetPrincipalFromExpiredToken(expiredToken);

        var email = principal.Claims.FirstOrDefault(
            claim => claim.Type == WsDecodedClaimTypes.Keys[JwtRegisteredClaimNames.Email])?.Value;
        
        return email;
    }

    public async Task<RefreshToken?> RefreshTokenAsync(
        User user, 
        string newToken, 
        string? oldToken = null, 
        CancellationToken? cancellationToken = null)
    {
        var tokenAlreadyUsed = user.UsedRefreshTokensFamily.Tokens
            .Any(token => token.Value == oldToken && token.IsUsed);
        if (tokenAlreadyUsed)
        {
            InvalidateRefreshToken(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            _logger.LogWarning(
                "An attempt to use token {OldToken} " +
                "(for user: id {UserId}, email {UserEmail}), " +
                "that is already used", 
                oldToken, user.Id, user.Email);
            
            throw new RefreshTokenUsedException($"{oldToken}");
        }
        
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

        return null;
    }

    public bool InvalidateRefreshToken(User user)
    {
        var refreshToken = user.RefreshToken;
        if (refreshToken is null)
        {
            return false;
        }

        refreshToken.IsUsed = true;
        user.UsedRefreshTokensFamily.Tokens.Add(refreshToken);
        user.RefreshToken = null;
        refreshToken.User = null;

        _unitOfWork.Users.Update(user);
        _refreshTokenRepository.Update(refreshToken);

        return true;
    }


    private async Task<RefreshToken?> UpdateRefreshTokenAsync(User user, string newToken, CancellationToken cancellationToken)
    {
        InvalidateRefreshToken(user);

        var refreshToken = RefreshTokenExtensions.CreateNotUsed(
            newToken,
            _jwtOptions.RefreshTokenLifetime);

        user.RefreshToken = refreshToken;
        user.UsedRefreshTokensFamily.Tokens.Add(refreshToken);

        _unitOfWork.Users.Update(user);
        await _refreshTokenRepository.CreateAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }
}