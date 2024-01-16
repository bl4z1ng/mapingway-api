using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Errors;
using Mapingway.Domain.Auth;
using Mapingway.SharedKernel;
using Mapingway.SharedKernel.Result;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Authentication.Token;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRepository<RefreshTokenFamily> _refreshTokenFamilies;
    private readonly IRepository<RefreshToken> _refreshTokens;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;

    public RefreshTokenService(IOptions<JwtOptions> jwtOptions, ITokenGenerator tokenGenerator, IUnitOfWork unitOfWork)
    {
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

        var newToken = _tokenGenerator.GenerateRandomToken();
        var newRefreshToken = RefreshToken.Create(newToken, _jwtOptions.RefreshTokenLifetime);

        user.RefreshTokensFamily.Tokens.Add(newRefreshToken);
        await _refreshTokens.CreateAsync(newRefreshToken, ct);
        _refreshTokenFamilies.Update(user.RefreshTokensFamily);

        return newRefreshToken;
    }

    public async Task<Result<RefreshToken>> RefreshTokenAsync(
        string email,
        string oldTokenKey,
        CancellationToken ct = default)
    {
        //TODO: refactor
        var user = await _users.GetByEmailWithRefreshTokensAsync(email, ct);
        if (user is null) return Result.Failure<RefreshToken>(UserError.NotFound);

        var oldToken = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldTokenKey);

        if (oldToken is null || oldToken.ExpiresAt < DateTime.UtcNow)
            return Result.Failure<RefreshToken>(TokenError.Expired);

        if (oldToken.IsUsed)
        {
            user.RefreshTokensFamily.InvalidateAllActiveTokens();
            await _unitOfWork.SaveChangesAsync(ct);

            throw new RefreshTokenUsedException(oldTokenKey, user.Id, user.Email);
        }

        user.RefreshTokensFamily.InvalidateToken(oldTokenKey);

        var newRefreshToken = RefreshToken.Create(
            value: _tokenGenerator.GenerateRandomToken(),
            lifetime: _jwtOptions.RefreshTokenLifetime);

        user.RefreshTokensFamily.Tokens.Add(newRefreshToken);

        await _refreshTokens.CreateAsync(newRefreshToken, ct);
        _refreshTokenFamilies.Update(user.RefreshTokensFamily);

        return newRefreshToken;
    }

    public async Task<Result> InvalidateTokenAsync(string userEmail, string oldToken, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailWithRefreshTokensAsync(userEmail, ct);
        if (user is null) return Result.Failure(UserError.NotFound);

        var token = user.RefreshTokensFamily.Tokens.FirstOrDefault(token => token.Value == oldToken);
        if (token is null) return Result.Failure(TokenError.NotFound);

        user.RefreshTokensFamily.InvalidateToken(token.Value);
        _refreshTokenFamilies.Update(user.RefreshTokensFamily);
        _refreshTokens.Update(token);

        return Result.Success();
    }
}
