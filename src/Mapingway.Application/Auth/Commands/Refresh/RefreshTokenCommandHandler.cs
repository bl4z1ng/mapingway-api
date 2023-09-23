using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth.Result;
using Mapingway.Common.Result;

namespace Mapingway.Application.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;


    public RefreshTokenCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
    }


    public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var userEmail = _authenticationService.GetEmailFromExpiredToken(command.ExpiredToken);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Email is not valid"));
        }

        var user = await _users.GetByEmailWithRefreshTokensAsync(userEmail, cancellationToken);
        if (user is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.NotFound, 
                "User not found"));
        }

        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        var activeRefreshToken = await _authenticationService.RefreshTokenAsync(
            user, 
            newRefreshToken, 
            command.RefreshToken, 
            cancellationToken);

        if (activeRefreshToken is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
        }
        
        var accessUnit = await _authenticationService.GenerateAccessToken(user.Id, user.Email);
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