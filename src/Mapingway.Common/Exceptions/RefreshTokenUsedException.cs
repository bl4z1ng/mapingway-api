namespace Mapingway.Common.Exceptions;

public class RefreshTokenUsedException : Exception
{
    public RefreshTokenUsedException(string token) : 
        base(message: $"Token {token}, that was passed for refresh, is already used.")
    {
    }
}