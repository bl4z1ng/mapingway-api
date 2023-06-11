using Mapingway.Common.Repository;
using Mapingway.Domain.User;
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

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IRepository<User> _usersRepository;

    public CreateUserCommandHandler(IRepository<User> userRepositoryRepository)
    {
        _usersRepository = userRepositoryRepository;
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