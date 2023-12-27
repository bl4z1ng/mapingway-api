namespace Mapingway.Infrastructure.Authentication.Exceptions;

public class RefreshTokenUsedException : Exception
{
    public RefreshTokenUsedException(string usedToken) : 
        base(message: $"Refresh token {usedToken} was already used, try to log-in again.")
    {
    }
}