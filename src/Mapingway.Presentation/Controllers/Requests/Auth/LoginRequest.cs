using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.Controllers.Requests.Auth;

public sealed class LoginRequest
{
    [Required]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
}