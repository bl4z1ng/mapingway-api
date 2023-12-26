using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;


    public LogoutCommandHandler(IUnitOfWork unitOfWork, IRefreshTokenService refreshTokenService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;

        _refreshTokenService = refreshTokenService;
    }


    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.DoesUserExistsByEmailAsync(request.Email, cancellationToken);
        if (!userExists)
        {
            return Result.Failure(new Error(
                ErrorCode.InvalidCredentials, 
                "Access token is invalid"));
        }

        var userHadActiveToken = 
            await _refreshTokenService.InvalidateTokenAsync(request.Email, request.RefreshToken);
        if (!userHadActiveToken)
        {
            return Result.Failure(new Error(
                ErrorCode.NotFound, 
                "User is not found or has no active refresh token, try to log-in again."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}