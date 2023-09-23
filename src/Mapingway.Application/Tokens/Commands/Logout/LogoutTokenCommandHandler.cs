using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Common.Result;

namespace Mapingway.Application.Tokens.Commands.Logout;

public class LogoutTokenCommandHandler : ICommandHandler<LogoutTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;


    public LogoutTokenCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;

        _authenticationService = authenticationService;
    }


    public async Task<Result> Handle(LogoutTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailWithRefreshTokensAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Failure(new Error(
                ErrorCode.InvalidCredentials, 
                "Access token is invalid"));
        }
        
        // TODO: add validation for token.isUsed + expired token
        var userHadActiveToken = _authenticationService.InvalidateRefreshToken(user);
        if (!userHadActiveToken)
        {
            return Result.Failure(new Error(
                ErrorCode.NotFound, 
                "User has no active refresh token, try to log-in again"));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}