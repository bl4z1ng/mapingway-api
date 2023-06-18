using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Common.Result;
using Mapingway.Domain.User;

namespace Mapingway.Application.Users.Commands.Register;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _usersRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var salt = _passwordHasher.GenerateSalt();
        var passwordHash = _passwordHasher.GenerateHash(request.Password, salt);
        
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordSalt = salt,
            PasswordHash = passwordHash,
            Role = "Biba"
        };

        var id =  await _usersRepository.CreateAsync(user, cancellationToken);

        return Result.Success(id);
    }
}