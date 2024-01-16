using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.Auth.Requests;

public record LogoutRequest
{
    /// <summary>
    /// Refresh token to be invalidated (also the user context cookie will be removed).
    /// </summary>
    /// <example>PwYuoPqGtW+Jd5aZJWrzUw==</example>
    [Required]
    public required string RefreshToken { get; init; } = null!;
}
