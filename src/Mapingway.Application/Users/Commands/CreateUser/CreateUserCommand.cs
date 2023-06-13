using Mapingway.Application.Messaging;
using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
        string Email, 
        string Password, 
        string Role, 
        string? FirstName, 
        string? LastName) : ICommand<int>;