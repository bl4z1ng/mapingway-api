﻿namespace Mapingway.Application.Contracts.Auth;

public class RefreshTokenResult
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public string UserContextToken { get; init; } = null!;
}