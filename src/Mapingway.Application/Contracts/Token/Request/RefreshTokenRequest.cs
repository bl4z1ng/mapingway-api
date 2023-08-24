using System.ComponentModel.DataAnnotations;

namespace Mapingway.Application.Contracts.Token.Request;

public class RefreshTokenRequest
{
    [Required]
    public string ExpiredToken { get; init; } = null!;
    [Required]
    public string RefreshToken { get; init; } = null!;
}