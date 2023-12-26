namespace Mapingway.Application.Contracts.Authentication;

public class AccessUnit
{
    //TODO: change to result
    public bool IsSuccess => AccessToken != null;
    public required string? AccessToken { get; init; }
    public required string UserContextToken { get; init; }
}