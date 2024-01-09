using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Errors;
using Mapingway.Application.Contracts.Messaging.Command;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;

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


    public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(command.Email, ct);
        if (user is null) return Result.Failure<RefreshTokenResult>(UserError.NotFound);

        var newRefreshTokenRequest =
            await _refreshTokenService.RefreshTokenAsync(user.Email, command.RefreshToken, ct);
        if (newRefreshTokenRequest.IsFailure) return Result.Failure<RefreshTokenResult>(newRefreshTokenRequest.Error);

        var accessTokenRequest = await _accessTokenService.GenerateAccessToken(user.Id, user.Email, ct);
        if (accessTokenRequest.IsFailure) return Result.Failure<RefreshTokenResult>(accessTokenRequest.Error);

        await _unitOfWork.SaveChangesAsync(ct);

        //TODO: Mapster
        return new RefreshTokenResult
        {
            Token = accessTokenRequest.Value!.AccessToken,
            RefreshToken = newRefreshTokenRequest.Value!.Value,
            UserContextToken = accessTokenRequest.Value!.UserContextToken
        };
    }
}
