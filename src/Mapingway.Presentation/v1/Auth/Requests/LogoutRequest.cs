using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.Auth.Requests;

public class LogoutRequest
{
    [Required]
    public string RefreshToken { get; init; } = null!;
}
