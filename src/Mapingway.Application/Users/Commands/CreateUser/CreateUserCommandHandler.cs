using Mapingway.Application.Messaging;
using Mapingway.Common.Repository;
using Mapingway.Common.Result;
using Mapingway.Domain.User;
using MediatR;

namespace Mapingway.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IRepository<User> _usersRepository;

    public CreateUserCommandHandler(IRepository<User> userRepository)
    {
        _usersRepository = userRepository;
    }
    
    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var id =  await _usersRepository.CreateAsync(user, cancellationToken);

        return Result.Success(id);
    }
}