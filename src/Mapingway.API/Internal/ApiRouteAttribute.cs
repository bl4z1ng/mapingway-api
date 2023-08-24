using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Internal;

public class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template) : base($"api/{template}")
    {
    }
}