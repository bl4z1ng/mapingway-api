using System.Security.Cryptography;
using System.Text;
using Mapingway.Application.Contracts.Authentication;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Security;

public class Hasher : IHasher
{
    private readonly HashOptions _hashOptions;


    public Hasher(IOptionsSnapshot<HashOptions> hashOptions)
    {
        _hashOptions = hashOptions.Value;
    }


    public string GenerateHash(string rawValue, string? salt = null)
    {
        var iterations = _hashOptions.Iterations;

        while (iterations > 0)
        {
            iterations--;

            var passwordSaltPepper = $"{rawValue}{salt ?? ""}{_hashOptions.Pepper}";
            var bytes = Encoding.UTF8.GetBytes(passwordSaltPepper);

            var byteHash = SHA256.HashData(bytes);

            rawValue = Convert.ToBase64String(byteHash);
        }

        return rawValue;
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
