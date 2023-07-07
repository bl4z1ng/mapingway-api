namespace Mapingway.Common.Exceptions;

public class RefreshTokenUsedException : Exception
{
    //TODO: get message into
    public RefreshTokenUsedException(string usedToken) : 
        base(message: $"Refresh token was already used, try to log-in again")
    {
    }
}