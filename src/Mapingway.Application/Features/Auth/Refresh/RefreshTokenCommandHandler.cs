using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;
    private readonly IAccessTokenService _accessTokenService;


    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService, 
        IAccessTokenService accessTokenService, 
        IUnitOfWork unitOfWork)
    {
        _refreshTokenService = refreshTokenService;
        _accessTokenService = accessTokenService;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
    }


    public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var userEmail = _accessTokenService.GetEmailFromExpiredToken(command.ExpiredToken);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Email is not valid"));
        }

        var user = await _users.GetByEmailAsync(userEmail, cancellationToken);
        if (user is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.NotFound, 
                "User not found"));
        }

        var activeRefreshToken = await _refreshTokenService.RefreshTokenAsync(
            user.Email, command.RefreshToken, cancellationToken);
        if (activeRefreshToken is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
        }
        
        var accessUnit = await _accessTokenService.GenerateAccessToken(user.Id, user.Email);
        if (!accessUnit.IsSuccess)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Failed to generate access token."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult
        {
            Token = accessUnit.AccessToken!,
            RefreshToken = activeRefreshToken.Value,
            UserContextToken = accessUnit.UserContextToken
        };
    }
}