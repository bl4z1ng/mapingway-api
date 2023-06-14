using Mapingway.Application.Messaging.Command;
using Mapingway.Application.Users.Interfaces;
using Mapingway.Common.Result;

namespace Mapingway.Application.Users.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;


    public LoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmail(request.Email, ct);

        if (user is null)
        {
            return Result.Failure<string>(new Error(
                "User.NotFound", 
                "User with given e-mail is not found"));
        }
        
        //check for password hash
        
        
        //generate jwt
        
        return "jwt-token";
    }
}