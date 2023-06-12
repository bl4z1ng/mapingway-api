using Mapingway.Application.Users.Commands.CreateUser;
using Mapingway.Common.Repository;
using Mapingway.Domain.User;
using MediatR;

namespace Mapingway.Application.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IRepository<User> _usersRepository;

    public CreateUserCommandHandler(IRepository<User> userRepository)
    {
        _usersRepository = userRepository;
    }
    
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _usersRepository.CreateAsync(user, cancellationToken);
    }
}