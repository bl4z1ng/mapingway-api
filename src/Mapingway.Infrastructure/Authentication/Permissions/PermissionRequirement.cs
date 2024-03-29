﻿using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication.Permissions;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission) { Permission = permission; }
    public string Permission { get; }
}
