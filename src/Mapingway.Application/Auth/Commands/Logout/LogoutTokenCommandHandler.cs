﻿using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Common.Result;

namespace Mapingway.Application.Auth.Commands.Logout;

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
        var userExists = await _userRepository.DoesUserExistsByEmailAsync(request.Email, cancellationToken);
        if (!userExists)
        {
            return Result.Failure(new Error(
                ErrorCode.InvalidCredentials, 
                "Access token is invalid"));
        }

        var userHadActiveToken = 
            await _authenticationService.InvalidateRefreshToken(request.Email, request.RefreshToken);
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