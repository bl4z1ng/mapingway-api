using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication.Permissions;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
    {
    }
}