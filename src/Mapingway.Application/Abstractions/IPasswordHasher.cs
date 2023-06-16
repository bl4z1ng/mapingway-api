namespace Mapingway.Application.Abstractions;

public interface IPasswordHasher
{
    string GenerateHash(string password, string salt);
    string GenerateSalt();
}