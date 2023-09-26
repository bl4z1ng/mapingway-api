namespace Mapingway.Application.Contracts.Abstractions.Authentication;

public interface IAccessTokenService
{
    string? GetEmailFromExpiredToken(string expiredToken);
}