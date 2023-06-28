using System.ComponentModel.DataAnnotations;

namespace Mapingway.Application.Contracts.User.Request;

public class RefreshRequest
{
    [Required]
    public string ExpiredToken { get; init; } = null!;

    [Required]
    public string RefreshToken { get; init; } = null!;
}