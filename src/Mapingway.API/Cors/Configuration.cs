namespace Mapingway.API.Cors;

public static class Configuration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string policyName)
    {
        //TODO: improve
        services.AddCors(opt =>
        {
            opt.AddPolicy(name: policyName, policyBuilder =>
            {
                policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}