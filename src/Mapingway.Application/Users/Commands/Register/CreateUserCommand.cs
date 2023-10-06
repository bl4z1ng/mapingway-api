﻿using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.User;

namespace Mapingway.Application.Users.Commands.Register;

public sealed record CreateUserCommand(
        string Email, 
        string Password, 
        string FirstName, 
        string? LastName) : ICommand<RegisterResult>;