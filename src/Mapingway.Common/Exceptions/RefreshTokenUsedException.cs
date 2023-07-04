namespace Mapingway.Common.Exceptions;

public class RefreshTokenUsedException : Exception
{
    public RefreshTokenUsedException(string message) : 
        base(message: $"The message {message} was recieved when validation refresh token, try to log-in again")
    {
    }
}