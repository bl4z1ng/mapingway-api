using System.Security.Cryptography;
using System.Text;
using Mapingway.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private readonly HashOptions _hashOptions;


    public PasswordHasher(IOptions<HashOptions> hashOptions)
    {
        _hashOptions = hashOptions.Value;
    }


    public string GenerateHash(string password, string? salt)
    {
        var iterations = _hashOptions.Iterations;
        
        while (iterations > 0)
        {
            iterations--;

            var passwordSaltPepper = $"{password}{salt ?? ""}{_hashOptions.Pepper}";
            var bytes = Encoding.UTF8.GetBytes(passwordSaltPepper);
            
            var byteHash = SHA256.HashData(bytes);
            
            password = Convert.ToBase64String(byteHash);
        }

        return password;
    }

    public string GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        
        rng.GetBytes(byteSalt);

        var salt = Convert.ToBase64String(byteSalt);

        return salt;
    }
}