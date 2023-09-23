using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Contracts;
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
    private readonly TokenValidationParameters _expiredTokenValidationParameters;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IHasher _hasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPermissionRepository _permissions;


    public AuthenticationService(
        ILoggerFactory loggerFactory,
        IOptions<JwtOptions> jwtOptions, 
        IOptions<TokenValidationParameters> tokenValidationOptions, 
        ITokenGenerator tokenGenerator,
        IHasher hasher,
        IUnitOfWork unitOfWork)
    {
        _logger = loggerFactory.CreateLogger(typeof(AuthenticationService));
        
        _jwtOptions = jwtOptions.Value;
        _tokenGenerator = tokenGenerator;
        _hasher = hasher;

        var validationParameters = tokenValidationOptions.Value;
        _expiredTokenValidationParameters = validationParameters.Clone();
        _expiredTokenValidationParameters.ValidateLifetime = false;

        _unitOfWork = unitOfWork;
        _refreshTokens = unitOfWork.RefreshTokens;
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
        claims.AddRange(permissions.Select(p => new Claim(CustomClaimNames.Permissions, p)));

        // TODO: refactor;
        var userContextToken = _tokenGenerator.GenerateRandomToken();
        var userContextHash = _hasher.GenerateHash(userContextToken, _jwtOptions.UserContextSalt);
        claims.Add(new Claim(CustomClaimNames.UserContext, userContextHash));

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

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.GenerateRandomToken();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        ClaimsPrincipal principal;

        try
        {
            principal = tokenValidationHandler.ValidateToken(
                expiredToken, 
                _expiredTokenValidationParameters, 
                out securityToken);
        }
        catch (ArgumentException e)
        {
            _logger.LogError("Recieved access token is invalid: {ExpiredToken}", expiredToken);
            throw new SecurityTokenException(message: "Recieved token is not valid", innerException: e);
        }
        // TODO: two exceptions?
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
        _refreshTokens.Update(refreshToken);

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
        await _refreshTokens.CreateAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }
}