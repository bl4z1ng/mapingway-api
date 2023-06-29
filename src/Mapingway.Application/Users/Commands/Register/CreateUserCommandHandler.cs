﻿using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.User.Result;
using Mapingway.Common.Result;
using Mapingway.Domain.Auth;
using Mapingway.Domain.User;

namespace Mapingway.Application.Users.Commands.Register;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, RegistrationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    
    private readonly IHasher _hasher;


    public CreateUserCommandHandler(IHasher hasher, IUnitOfWork unitOfWork)
    {
        _hasher = hasher;
        
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
    }


    public async Task<Result<RegistrationResult>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
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

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegistrationResult
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user?.LastName ?? string.Empty
        };
    }
}