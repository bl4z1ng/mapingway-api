namespace Mapingway.Application.Contracts.Authentication;

public interface IHasher
{
    string GenerateHash(string rawValue, string? salt = null);
    string GenerateSalt();
}
