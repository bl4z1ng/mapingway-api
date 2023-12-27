namespace Mapingway.Application.Contracts.Authentication;

public class AccessUnit
{
    public required string AccessToken { get; init; }
    public required string UserContextToken { get; init; }
}
