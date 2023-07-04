namespace Mapingway.Common.Constants;

public static class ExceptionMessage
{
    public const string HttpRequestException = "Request Exception";
    public const string UsedRefreshTokenException = "Got an refresh token, that is already used, log-in again";
    public const string DatabaseUpdateException = "Request Exception while updating Database";
    public const string NullReferenceUserException = "Cannot find user";
    public const string UserValidationException = "Validation Exception";
    public const string UserAuthorizedException = "Password did not match";
    public const string UnauthorizedException = "Authorization exception";
    public const string ServerErrorException = "Internal Server Error";
}