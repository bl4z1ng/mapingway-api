using Mapingway.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication.Permission;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permissions) : base(policy: permissions.ToString()) 
    {
    }
}