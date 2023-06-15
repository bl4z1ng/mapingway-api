using Mapingway.Application.Abstractions;
using Mapingway.Application.Messaging.Command;
using Mapingway.Common.Result;

namespace Mapingway.Application.Users.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    //private readonly IPasswordHasher _passwordHasher;


    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider) //, IPasswordHasher passwordHasher
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        //_passwordHasher = passwordHasher;
    }


    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmail(request.Email, ct);

        if (user is null)
        {
            return Result.Failure<string>(new Error(
                "404", 
                "User with given e-mail is not found"));
        }
        
        //check for password hash
        //var hashedPassword = _passwordHasher.EncryptPassword(request.Password);
        
        //generate jwt
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}