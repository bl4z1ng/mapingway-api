using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Errors;
using Mapingway.Domain.Auth;
using Mapingway.Infrastructure.Authentication.Exceptions;
using Mapingway.SharedKernel.Result;
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


    public async Task<Result<RefreshToken>> CreateTokenAsync(string email, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(email, ct);
        if (user is null) return Result.Failure<RefreshToken>(UserError.NotFound);

        var newRefreshToken = _tokenGenerator.GenerateRandomToken();

        return await AddRefreshTokenAsync(user.RefreshTokensFamily, newRefreshToken, ct);
    }

    public async Task<Result<RefreshToken>> RefreshTokenAsync(
        string email,
        string oldTokenKey, CancellationToken ct = default)
    {
        //TODO: refactor
        var user = await _users.GetByEmailWithRefreshTokensAsync(email, ct);
        if (user is null) return Result.Failure<RefreshToken>(UserError.NotFound);

        var oldToken = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldTokenKey);

        if (oldToken is null || oldToken.ExpiresAt < DateTime.UtcNow) return null;//TODO: pass error
        if (oldToken.IsUsed)
        {
            user.RefreshTokensFamily.InvalidateAllActiveTokens();
            await _unitOfWork.SaveChangesAsync(ct);

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
            ct: ct);

        return result;
    }

    public async Task<Result> InvalidateTokenAsync(string userEmail, string oldToken, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(userEmail, ct);
        if (user is null) return Result.Failure(UserError.NotFound);

        var token = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldToken);
        if (token is null) return Result.Failure(TokenError.NotFound);

        user.RefreshTokensFamily.InvalidateRefreshToken(token.Value);

        _refreshTokenFamilies.Update(user.RefreshTokensFamily);
        _refreshTokens.Update(token);

        return Result.Success();
    }


    private async Task<Result<RefreshToken>> AddRefreshTokenAsync(
        RefreshTokenFamily tokenFamily,
        string newTokenKey,
        CancellationToken ct = default)
    {
        var newRefreshToken = RefreshTokenExtensions.CreateNotUsed(
            newTokenKey,
            _jwtOptions.RefreshTokenLifetime);

        tokenFamily.Tokens.Add(newRefreshToken);

        await _refreshTokens.CreateAsync(newRefreshToken, ct);
        _refreshTokenFamilies.Update(tokenFamily);

        return newRefreshToken;
    }
}
