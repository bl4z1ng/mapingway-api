namespace Mapingway.Application.Contracts.Abstractions;

public interface IHasher
{
    string GenerateHash(string rawValue, string? salt = null);
    string GenerateSalt();
}