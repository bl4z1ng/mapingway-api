using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Authentication;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        if(!int.TryParse(userId, out var parsedUserId))
        {
            return;
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var permissions = await permissionService.GetPermissions(parsedUserId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}