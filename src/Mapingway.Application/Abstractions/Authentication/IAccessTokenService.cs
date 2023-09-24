namespace Mapingway.Application.Abstractions.Authentication;

public interface IAccessTokenService
{
    string? GetEmailFromExpiredToken(string expiredToken);
}