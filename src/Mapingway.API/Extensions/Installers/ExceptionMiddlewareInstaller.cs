using Mapingway.API.Middleware;

namespace Mapingway.API.Extensions.Installers;

public static class ExceptionMiddlewareInstaller
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}