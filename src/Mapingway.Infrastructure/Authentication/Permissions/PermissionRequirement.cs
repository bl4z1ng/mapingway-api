﻿using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication.Permissions;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }


    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}