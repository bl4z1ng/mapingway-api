using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Domain.Auth;
using Mapingway.Infrastructure.Authentication.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Authentication.Token;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger _logger;
    private readonly IUsedRefreshTokenFamilyRepository _refreshTokenFamilies;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;


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


    public async Task<RefreshToken?> CreateTokenAsync(
        string email, 
        CancellationToken? cancellationToken = null)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(email, cancellationToken);
        if (user is null) return null;
        
        var newToken = _tokenGenerator.GenerateRandomToken();
        
        return await AddRefreshTokenAsync(
            user.RefreshTokensFamily,
            newToken,
            cancellationToken: cancellationToken ?? CancellationToken.None);
    }

    public async Task<RefreshToken?> RefreshTokenAsync(
        string email, 
        string oldTokenKey, 
        CancellationToken? cancellationToken = null)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(email, cancellationToken);
        if (user is null) return null;

        var oldToken = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldTokenKey);

        if (oldToken is null || oldToken.ExpiresAt < DateTime.UtcNow) return null;
        if (oldToken.IsUsed)
        {
            user.RefreshTokensFamily.InvalidateAllActiveTokens();
            await _unitOfWork.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            _logger.LogWarning(
                "An attempt to use token {OldToken} (for user: id {UserId}, email {UserEmail}), " +
                "that is already used", oldTokenKey, user.Id, user.Email);
        
            throw new RefreshTokenUsedException($"{oldTokenKey}");
        }

        user.RefreshTokensFamily.InvalidateRefreshToken(oldTokenKey);

        var newToken = _tokenGenerator.GenerateRandomToken();
        var result = await AddRefreshTokenAsync(
            user.RefreshTokensFamily,
            newToken,
            cancellationToken: cancellationToken ?? CancellationToken.None);

        return result;
    }

    public async Task<bool> InvalidateTokenAsync(
        string userEmail, 
        string oldTokenKey, 
        CancellationToken? ct = null)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(userEmail, ct ?? CancellationToken.None);
        if (user is null) return false;

        var token = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldTokenKey);
        if (token is null) return false;

        user.RefreshTokensFamily.InvalidateRefreshToken(token.Value);

        _refreshTokenFamilies.Update(user.RefreshTokensFamily);
        _refreshTokens.Update(token);

        return true;
    }


    private async Task<RefreshToken?> AddRefreshTokenAsync(
        RefreshTokenFamily tokenFamily, 
        string newTokenKey,
        CancellationToken? cancellationToken = null)
    {
        var newRefreshToken = RefreshTokenExtensions.CreateNotUsed(
            newTokenKey,
            _jwtOptions.RefreshTokenLifetime);

        tokenFamily.Tokens.Add(newRefreshToken);

        await _refreshTokens.CreateAsync(newRefreshToken, cancellationToken);
        _refreshTokenFamilies.Update(tokenFamily);

        return newRefreshToken;
    }
}