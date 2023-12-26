namespace Mapingway.Application.Contracts.Authentication.Results;

public class AccessUnit
{
    public bool IsSuccess => AccessToken != null;
    public string? AccessToken { get; init; }
    public string UserContextToken { get; init; } = null!;
}