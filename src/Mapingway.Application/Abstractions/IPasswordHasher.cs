namespace Mapingway.Application.Abstractions;

public interface IPasswordHasher
{
    string EncryptPassword(string pass);
}