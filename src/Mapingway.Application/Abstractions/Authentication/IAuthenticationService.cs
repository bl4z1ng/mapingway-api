﻿using Mapingway.Application.Contracts;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<AccessUnit> GenerateAccessToken(long userId, string email, CancellationToken? cancellationToken = null);
    string GenerateRefreshToken();
    Task<RefreshToken?> UpdateRefreshTokenAsync(
        string email, 
        string newTokenValue, 
        string? oldTokenValue = null, 
        CancellationToken? cancellationToken = null);
    Task<bool> InvalidateRefreshToken(string email, string refreshToken, CancellationToken? ct = null);
}