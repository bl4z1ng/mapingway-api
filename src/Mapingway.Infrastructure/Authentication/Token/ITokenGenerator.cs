namespace Mapingway.Infrastructure.Authentication.Token;

public interface ITokenGenerator
{
    string? GenerateAccessToken(AccessTokenDetails details);

    string GenerateRandomToken(int numberOfBytes = 16);
}