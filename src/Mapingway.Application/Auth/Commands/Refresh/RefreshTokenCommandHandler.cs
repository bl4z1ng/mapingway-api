﻿using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth;
using Mapingway.Common.Result;

namespace Mapingway.Application.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;
    private readonly IAccessTokenService _accessTokenService;


    public RefreshTokenCommandHandler(
        IAuthenticationService authenticationService, 
        IAccessTokenService accessTokenService, 
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _accessTokenService = accessTokenService;

        _unitOfWork = unitOfWork;
        _users = unitOfWork.Users;
    }


    public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var userEmail = _accessTokenService.GetEmailFromExpiredToken(command.ExpiredToken);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Email is not valid"));
        }

        var user = await _users.GetByEmailAsync(userEmail, cancellationToken);
        if (user is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.NotFound, 
                "User not found"));
        }

        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        var activeRefreshToken = await _authenticationService.UpdateRefreshTokenAsync(
            user.Email, 
            newRefreshToken, 
            command.RefreshToken, 
            cancellationToken);

        if (activeRefreshToken is null)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
        }
        
        var accessUnit = await _authenticationService.GenerateAccessToken(user.Id, user.Email);
        if (!accessUnit.IsSuccess)
        {
            return Result.Failure<RefreshTokenResult>(new Error(
                ErrorCode.InvalidCredentials, 
                "Failed to generate access token."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult
        {
            Token = accessUnit.AccessToken!,
            RefreshToken = activeRefreshToken.Value,
            UserContextToken = accessUnit.UserContextToken
        };
    }
}