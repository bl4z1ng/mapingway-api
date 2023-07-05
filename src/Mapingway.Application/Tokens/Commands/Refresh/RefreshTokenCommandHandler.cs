using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Token.Result;
using Mapingway.Common.Constants;
using Mapingway.Common.Result;

namespace Mapingway.Application.Tokens.Commands.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;
    private readonly IPermissionRepository _permissions;


    public RefreshTokenCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
        _permissions = _unitOfWork.Permissions;
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
            command.RefreshToken, 
            newRefreshToken, 
            cancellationToken);

        if (activeRefreshToken is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
        }
        
        var permissions = await _permissions.GetPermissionsAsync(user.Id, cancellationToken);
        var token = _authenticationService.GenerateAccessToken(user, permissions);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult
        {
            Token = token,
            RefreshToken = activeRefreshToken.Value
        };
    }
}