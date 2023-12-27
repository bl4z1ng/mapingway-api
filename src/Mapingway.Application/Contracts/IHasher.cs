namespace Mapingway.Application.Contracts;

public interface IHasher
{
    string GenerateHash(string rawValue, string? salt = null);
    string GenerateSalt();
}
