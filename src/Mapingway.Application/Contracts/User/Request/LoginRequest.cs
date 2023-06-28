using System.ComponentModel.DataAnnotations;

namespace Mapingway.Application.Contracts.User.Request;

public sealed class LoginRequest
{
    [Required]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
}