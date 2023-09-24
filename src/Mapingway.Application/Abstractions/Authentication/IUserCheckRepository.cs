namespace Mapingway.Application.Abstractions.Authentication;

public interface IUserCheckRepository
{
    Task<bool> DoesUserExistsByEmailAsync(string email, CancellationToken? ct = null);
}