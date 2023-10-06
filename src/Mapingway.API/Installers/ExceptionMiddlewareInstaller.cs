using Mapingway.API.Middleware;
using Mapingway.API.Middleware.Exception;

namespace Mapingway.API.Installers;

public static class ExceptionMiddlewareInstaller
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}