using System.Security.Claims;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAccessTokenParser
{
    ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
    string? GetEmailFromExpiredToken(string expiredToken);
}