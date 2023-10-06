using System.ComponentModel.DataAnnotations;

namespace Mapingway.API.Controllers.Requests.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string ExpiredToken { get; init; } = null!;
    [Required]
    public string RefreshToken { get; init; } = null!;
}