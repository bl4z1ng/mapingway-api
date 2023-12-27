namespace Mapingway.API.Middleware.Exception;

public static class ExceptionMessage
{
    //TODO: get rid of, when moving to ProblemDetails
    public const string HttpRequestException = "Request Exception.";
    public const string UsedRefreshTokenException = "Got a refresh token, that is already used, log-in again.";
    public const string InvalidAccessToken = "Got an access token, that is not valid, try again.";
    public const string ServerErrorException = "Internal Server Error.";
}
