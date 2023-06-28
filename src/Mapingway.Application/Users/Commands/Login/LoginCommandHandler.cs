using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.User.Result;
using Mapingway.Common.Result;

namespace Mapingway.Application.Users.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    private readonly IJwtService _jwtService;
    private readonly IPermissionService _permissionService;


    public LoginCommandHandler(
        IUserRepository userRepository, 
        IHasher hasher,
        IJwtService jwtService,
        IPermissionService permissionService)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _jwtService = jwtService;
        _permissionService = permissionService;
    }


    public async Task<Result<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
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

        var permissions = await _permissionService.GetPermissionsAsync(user.Id, cancellationToken);
        
        var tokenPair = await _jwtService.GenerateTokensAsync(user, permissions, cancellationToken);

        return tokenPair;
    }
}