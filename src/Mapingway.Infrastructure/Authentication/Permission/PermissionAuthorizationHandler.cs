using Mapingway.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication.Permission;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var permissions = context.User.Claims
            .Where(p => p.Type == CustomClaimNames.Permissions)
            .Select(p => p.Value)
            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}