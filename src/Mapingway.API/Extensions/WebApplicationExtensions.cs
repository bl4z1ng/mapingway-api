namespace Mapingway.API.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseSwaggerDarkTheme(this IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet("/swagger-dark.css");
        });

        return app;
    }
}