using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.Auth.Requests;

public sealed class LoginRequest
{
    /// <summary>
    /// The email, will be used as unique identifier.
    /// </summary>
    /// <example>max.pyte@gmail.com</example>
    [Required]
    public required string Email { get; init; }

    /// <summary>
    /// Password must contain at least 3 letter and be within limits of [8,30] (inclusive).
    /// </summary>
    /// <example>Password</example>
    [Required]
    public required string Password { get; init; }
}
