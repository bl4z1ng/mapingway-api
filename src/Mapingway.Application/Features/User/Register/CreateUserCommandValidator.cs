using FluentValidation;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Application.Features.User.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(c => c.FirstName)
            .NotEmpty()
            //TODO: think about common number for all validation rules
            .MaximumLength(256);

        RuleFor(c => c.Email)
            .NotEmpty()
            .ValidEmail()
            .MustAsync(async (email, ct) => await userRepository.GetByEmailAsync(email, ct) is null)
            .WithMessage("Email is already taken.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .HasLetters(3);
    }
}