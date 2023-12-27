using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.Auth.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string ExpiredToken { get; init; } = null!;

    [Required]
    public string RefreshToken { get; init; } = null!;
}