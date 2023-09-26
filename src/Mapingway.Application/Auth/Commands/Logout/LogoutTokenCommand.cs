﻿using Mapingway.Application.Contracts.Abstractions.Messaging.Command;

namespace Mapingway.Application.Auth.Commands.Logout;

public record LogoutTokenCommand(string Email, string RefreshToken) : ICommand;