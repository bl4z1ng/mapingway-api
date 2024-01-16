using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.Auth.Requests;

public record RefreshTokenRequest
{
    /// <summary>
    /// JWT Bearer token, used as an access token (expired or not).
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z.f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A</example>
    [Required]
    public required string ExpiredToken { get; init; } = null!;

    /// <summary>
    /// Refresh token to get the new access + refresh token pair, one-time use.
    /// </summary>
    /// <example>PwYuoPqGtW+Jd5aZJWrzUw==</example>
    [Required]
    public required string RefreshToken { get; init; } = null!;
}

