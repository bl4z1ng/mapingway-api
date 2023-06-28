using Mapingway.Application.Contracts.User.Result;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IJwtService
{
    Task<AuthenticationResult> GenerateTokensAsync(User user, IEnumerable<string> permissions, CancellationToken ct);
}