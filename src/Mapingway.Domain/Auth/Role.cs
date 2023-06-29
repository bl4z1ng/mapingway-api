using Mapingway.Domain.User;

namespace Mapingway.Domain.Auth;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Permission> Permissions { get; set; } = null!;
    public ICollection<User.User> Users { get; set; } = null!;


    public static Role User => new() { Id = 1, Name = nameof(User) };
    public static Role Admin => new() { Id = 2, Name = nameof(Admin) };

    public static IEnumerable<Role> GetValues()
    {
        return new List<Role>
        {
            User,
            Admin
        };
    }
}