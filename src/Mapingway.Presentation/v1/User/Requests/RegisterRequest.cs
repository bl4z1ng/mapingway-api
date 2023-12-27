using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.User.Requests;

public class RegisterRequest
{
    /// <summary>
    /// The email, will be used as unique identifier.
    /// </summary>
    /// <example>max.pyte@gmail.com</example>
    [Required]
    public required string Email { get; init; } = null!;

    /// <summary>
    /// Password must contain at least 3 letter and be within limits of [8,30] (inclusive).
    /// </summary>
    /// <example>Password</example>
    [Required]
    public required string Password { get; init; } = null!;

    /// <summary>
    /// First name will be shown in profile and used as human-readable id, maximum length is 256 symbols.
    /// </summary>
    /// <example>Max</example>
    [Required]
    public required string FirstName { get; init; } = null!;

    /// <summary>
    /// Last name will be shown in profile and optional, maximum length is 256 symbols.
    /// </summary>
    /// <example>Pyte</example>
    public string? LastName { get; init; }
}
