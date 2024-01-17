using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Errors;
using Mapingway.Application.Contracts.Messaging.Command;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
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


    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null) return Result.Failure<LoginResult>(UserError.NotFound);

        var passwordHash = _hasher.GenerateHash(request.Password, user.PasswordSalt!);
        if (passwordHash != user.PasswordHash) return Result.Failure<LoginResult>(AuthError.InvalidPassword);

        var refreshTokenRequest = await _refreshTokenService.CreateTokenAsync(user.Email, ct);
        if (refreshTokenRequest.IsFailure) return Result.Failure<LoginResult>(refreshTokenRequest.Error);

        var accessTokenRequest = await _accessTokenService.GenerateAccessToken(user.Id, user.Email, ct);
        if (accessTokenRequest.IsFailure) return Result.Failure<LoginResult>(accessTokenRequest.Error);

        await _unitOfWork.SaveChangesAsync(ct);

        return new LoginResult
        {
            Token = accessTokenRequest.Value!.AccessToken!,
            UserContextToken = accessTokenRequest.Value!.UserContextToken,
            RefreshToken = refreshTokenRequest.Value!.Value
        };
    }
}
