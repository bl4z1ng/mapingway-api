using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Messaging.Command;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.Auth.Logout;

// ReSharper disable once UnusedType.Global
public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IRefreshTokenService refreshTokenService, IUnitOfWork unitOfWork)
    {
        _refreshTokenService = refreshTokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var invalidateRequest = await _refreshTokenService.InvalidateTokenAsync(request.Email, request.RefreshToken);
        if (invalidateRequest.IsFailure) return Result.Failure(invalidateRequest.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
