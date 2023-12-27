using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthenticationResult>
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IHasher _hasher;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;


    public LoginCommandHandler(
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        IHasher hasher,
        IUnitOfWork unitOfWork)
    {
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _hasher = hasher;

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

        var activeRefreshToken = await _refreshTokenService.CreateTokenAsync(user.Email, cancellationToken);
        if (activeRefreshToken is null)
        {
            return Result.Failure<AuthenticationResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again."));
        }
        
        var accessUnit = await _accessTokenService.GenerateAccessToken(user.Id, user.Email, cancellationToken);
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