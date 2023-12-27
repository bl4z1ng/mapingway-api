using System.ComponentModel.DataAnnotations;

namespace Mapingway.Presentation.v1.User.Requests;

public class RegisterRequest
{
    [Required] 
    public string Email { get; init; } = null!;

    [Required] 
    public string Password { get; init; } = null!;

    [Required] 
    public string FirstName { get; init; } = null!;

    public string? LastName { get; init; }
}