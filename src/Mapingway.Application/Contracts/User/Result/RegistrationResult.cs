﻿namespace Mapingway.Application.Contracts.User.Result;

public class RegistrationResult
{
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}