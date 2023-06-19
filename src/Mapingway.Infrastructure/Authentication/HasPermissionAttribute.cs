﻿using Mapingway.Common.Permission;
using Microsoft.AspNetCore.Authorization;

namespace Mapingway.Infrastructure.Authentication;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permissions) : base(policy: permissions.ToString()) 
    {
    }
}