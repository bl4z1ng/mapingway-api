namespace Mapingway.Application.Contracts.Authentication;

public interface IUserCheckRepository
{
    Task<bool> DoesUserExistsByEmailAsync(string email, CancellationToken ct = default);
}
