using Microsoft.AspNetCore.Mvc;

namespace Mapingway.Presentation.Controllers;

public class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template) : base($"api/{template}")
    {
    }
}