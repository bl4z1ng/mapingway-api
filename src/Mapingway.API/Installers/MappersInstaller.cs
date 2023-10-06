using Mapingway.API.Internal.Mapping;

namespace Mapingway.API.Installers;

public static class MappersInstaller
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IRequestToCommandMapper, MapperlyRequestToCommandMapper>();
        services.AddScoped<IResultToResponseMapper, MapperlyResultToResponseMapper>();

        return services;
    }
}