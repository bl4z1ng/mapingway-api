using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth;
using Mapingway.Common.Result;

namespace Mapingway.Application.Auth.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthenticationResult>
{
    private readonly IHasher _hasher;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;


    public LoginCommandHandler(
        IHasher hasher,
        IAuthenticationService authenticationService,
        IUnitOfWork unitOfWork)
    {
        _hasher = hasher;
        _authenticationService = authenticationService;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
    }


    public async Task<Result<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Failure<AuthenticationResult>(new Error(
                ErrorCode.NotFound, 
                "User with given e-mail is not found."));
        }

        var passwordHash = _hasher.GenerateHash(request.Password, user.PasswordSalt!);
        if (passwordHash != user.PasswordHash)
        {
            return Result.Failure<AuthenticationResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Email or password is incorrect."));
        }

        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        var activeRefreshToken = await _authenticationService
            .UpdateRefreshTokenAsync(user.Email, newRefreshToken, null, cancellationToken);
        if (activeRefreshToken is null)
        {
            return Result.Failure<AuthenticationResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again."));
        }
        
        var accessUnit = await _authenticationService.GenerateAccessToken(user.Id, user.Email, cancellationToken);
        if (!accessUnit.IsSuccess)
        {
            return Result.Failure<AuthenticationResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Failed to generate access token."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AuthenticationResult
        {
            Token = accessUnit.AccessToken!,
            UserContextToken = accessUnit.UserContextToken,
            RefreshToken = activeRefreshToken.Value
        };
    }
}