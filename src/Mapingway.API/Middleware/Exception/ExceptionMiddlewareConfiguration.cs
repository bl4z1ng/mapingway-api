using Microsoft.AspNetCore.Builder;

namespace Mapingway.API.Middleware.Exception;

public static class ExceptionMiddlewareConfiguration
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}