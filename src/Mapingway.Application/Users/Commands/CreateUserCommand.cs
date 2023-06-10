using MediatR;

namespace Mapingway.Application.Users.Commands;

public class CreateUserCommand : IRequest
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}

// public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
// {
//     public CreateUserCommandHandler(IRepository<User> userRepository, ILoggerFactory loggerFactory)
//     {
//         
//     }
//     
//     public Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
//     {
//         throw new NotImplementedException();
//     }
// }