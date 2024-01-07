using Microsoft.AspNetCore.Http;

namespace Mapingway.Infrastructure.Logging;

public class CorrelationIdMiddleware
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CorrelationIdMiddleware(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.ContainsKey("correlation-id"))
        {
        }

        await next(context);
    }
}
