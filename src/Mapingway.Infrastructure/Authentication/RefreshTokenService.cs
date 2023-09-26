using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mapingway.Domain.Auth;
using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Infrastructure.Authentication.Exceptions;
using Mapingway.Infrastructure.Authentication.Token;

namespace Mapingway.Infrastructure.Authentication;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ILogger _logger;
    private readonly JwtOptions _jwtOptions;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUserRepository _users;
    private readonly IUsedRefreshTokenFamilyRepository _refreshTokenFamilies;


    public RefreshTokenService(
        ILoggerFactory loggerFactory,
        IOptions<JwtOptions> jwtOptions, 
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _logger = loggerFactory.CreateLogger<RefreshTokenService>();
        
        _jwtOptions = jwtOptions.Value;
        _tokenGenerator = tokenGenerator;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
        _refreshTokens = unitOfWork.RefreshTokens;
        _refreshTokenFamilies = unitOfWork.RefreshTokenFamilies;
    }

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.GenerateRandomToken();
    }

    public async Task<RefreshToken?> UpdateRefreshTokenAsync(
        string email, 
        string newTokenValue, 
        string? oldTokenValue = null, 
        CancellationToken? cancellationToken = null)
    {
        RefreshToken? newRefreshToken;

        var user = await _users.GetByEmailWithRefreshTokensAsync(email, cancellationToken);
        if (user is null) return null;

        // if login
        if (oldTokenValue is null)
        {
            newRefreshToken = await UpdateRefreshTokenInternalAsync(
                user.RefreshTokensFamily, 
                newTokenValue, 
                cancellationToken: cancellationToken ?? CancellationToken.None);

            return newRefreshToken;
        }
        
        // if no such token or it has been expired
        var refreshTokenByKey = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldTokenValue);
        if (refreshTokenByKey is null || refreshTokenByKey.ExpiresAt < DateTime.UtcNow)
        {
            return null;
        }
        
        // if token was used
        if (refreshTokenByKey.IsUsed)
        {
            user.RefreshTokensFamily.InvalidateAllActiveUserTokens();
            await _unitOfWork.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            _logger.LogWarning(
                "An attempt to use token {OldToken} (for user: id {UserId}, email {UserEmail}), " +
                "that is already used", oldTokenValue, user.Id, user.Email);
            
            throw new RefreshTokenUsedException($"{oldTokenValue}");
        }

        newRefreshToken = await UpdateRefreshTokenInternalAsync(
            user.RefreshTokensFamily,
            newTokenValue,
            oldRefreshTokenKey: refreshTokenByKey.Value,
            cancellationToken: cancellationToken ?? CancellationToken.None);

        return newRefreshToken;
    }

    public async Task<bool> InvalidateRefreshToken(
        string userEmail, 
        string refreshTokenKey, 
        CancellationToken? ct = null)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(userEmail, ct ?? CancellationToken.None);
        if (user is null) return false;

        var token = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == refreshTokenKey);
        if (token is null) return false;

        user.RefreshTokensFamily.InvalidateRefreshToken(token.Value);

        _refreshTokenFamilies.Update(user.RefreshTokensFamily);
        _refreshTokens.Update(token);

        return true;
    }


    private async Task<RefreshToken?> UpdateRefreshTokenInternalAsync(
        RefreshTokenFamily tokenFamily, 
        string newRefreshTokenKey, 
        string? oldRefreshTokenKey = null, 
        CancellationToken? cancellationToken = null)
    {
        if (oldRefreshTokenKey is not null)
        {
            tokenFamily.InvalidateRefreshToken(oldRefreshTokenKey);
        }

        var newRefreshToken = RefreshTokenExtensions.CreateNotUsed(
            newRefreshTokenKey,
            _jwtOptions.RefreshTokenLifetime);

        tokenFamily.Tokens.Add(newRefreshToken);

        await _refreshTokens.CreateAsync(newRefreshToken, cancellationToken);
        _refreshTokenFamilies.Update(tokenFamily);

        return newRefreshToken;
    }
}