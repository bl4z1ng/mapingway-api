﻿using Mapingway.Domain;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAuthenticationService : IAccessTokenParser
{
    string GenerateAccessToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    Task<RefreshToken?> RefreshTokenAsync(User user, string newRefreshToken, string? oldRefreshToken = null, CancellationToken? cancellationToken = null);
    public bool InvalidateRefreshToken(User user);
}