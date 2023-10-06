namespace Mapingway.API.Internal.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseSwaggerUIDark(this IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet(@"/swagger-dark.css");
        });

        return app;
    }
}