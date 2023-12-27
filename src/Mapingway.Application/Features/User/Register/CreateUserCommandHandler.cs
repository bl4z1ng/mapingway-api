using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Domain.Auth;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Features.User.Register;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, RegisterResult>
{
    private readonly IHasher _hasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IHasher hasher, IUnitOfWork unitOfWork)
    {
        _hasher = hasher;

        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
    }

    public async Task<Result<RegisterResult>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var salt = _hasher.GenerateSalt();
        var passwordHash = _hasher.GenerateHash(command.Password, salt);

        var user = new Domain.User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordSalt = salt,
            PasswordHash = passwordHash,
            Roles = new List<Role> { Role.User }
        };

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterResult
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName ?? string.Empty
        };
    }
}
