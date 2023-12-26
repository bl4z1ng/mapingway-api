using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.Controllers.Requests.Auth;

public class LogoutRequest
{
    [Required]
    public string RefreshToken { get; init; } = null!;
}