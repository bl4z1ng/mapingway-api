using System.ComponentModel.DataAnnotations;

namespace Mapingway.Application.Contracts.User;

public sealed class LoginRequest
{
    [Required]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
}