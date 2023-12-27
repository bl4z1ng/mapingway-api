namespace Mapingway.Presentation.v1.Auth.Responses;

public class AccessTokenResponse
{
    /// <summary>
    /// JWT Bearer token, used as an access token (expired or not).
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z.f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A</example>
    public required string Token { get; init; } = null!;

    /// <summary>
    /// Refresh token to get the new access + refresh token pair, one-time use.
    /// </summary>
    /// <example>PwYuoPqGtW+Jd5aZJWrzUw==</example>
    public required string RefreshToken { get; init; } = null!;
}
