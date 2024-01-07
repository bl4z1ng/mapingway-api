using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Mapingway.Infrastructure.Logging.CorrelationToken;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next){ _next = next; }

    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.Request.Headers.GetCorrelationId();
        if (id is null)
        {
            id = Guid.NewGuid().ToString();
            context.Request.Headers.AddCorrelationId(id);
        }

        // Propagate to response
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.AddCorrelationId(id);

            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationToken(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
