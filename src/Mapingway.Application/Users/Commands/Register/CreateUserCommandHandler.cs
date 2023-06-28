using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Common.Result;
using Mapingway.Domain.User;

namespace Mapingway.Application.Users.Commands.Register;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _usersRepository;
    private readonly IHasher _hasher;


    public CreateUserCommandHandler(IUserRepository userRepository, IHasher hasher)
    {
        _usersRepository = userRepository;
        _hasher = hasher;
    }


    public async Task<Result<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var salt = _hasher.GenerateSalt();
        var passwordHash = _hasher.GenerateHash(command.Password, salt);

        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordSalt = salt,
            PasswordHash = passwordHash,
            Roles = new List<Role> { Role.User }
        };

        var id =  await _usersRepository.CreateAsync(user, cancellationToken);

        return id;
    }
}