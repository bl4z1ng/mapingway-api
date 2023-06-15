using Mapingway.Application.Abstractions;
using Mapingway.Application.Messaging.Command;
using Mapingway.Common.Repository;
using Mapingway.Common.Result;
using Mapingway.Domain.User;

namespace Mapingway.Application.Users.Commands.Register;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _usersRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
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
        await _usersRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(id);
    }
}