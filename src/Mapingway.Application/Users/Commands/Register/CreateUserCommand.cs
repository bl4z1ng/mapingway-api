﻿using Mapingway.Application.Messaging.Command;

namespace Mapingway.Application.Users.Commands.Register;

public sealed record CreateUserCommand(
        string Email, 
        string Password, 
        string Role, 
        string? FirstName, 
        string? LastName) : ICommand<int>;