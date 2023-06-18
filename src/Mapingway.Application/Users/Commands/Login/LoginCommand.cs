using Mapingway.Application.Abstractions.Messaging.Command;

namespace Mapingway.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<string>
{
}