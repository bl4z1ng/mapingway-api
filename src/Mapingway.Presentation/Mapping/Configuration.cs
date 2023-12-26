using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Presentation.Mapping;

public static class Configuration
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IRequestToCommandMapper, MapperlyRequestToCommandMapper>();
        services.AddScoped<IResultToResponseMapper, MapperlyResultToResponseMapper>();

        return services;
    }
}