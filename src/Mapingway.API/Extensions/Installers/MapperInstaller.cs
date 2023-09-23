using Mapingway.API.Internal.Mapping;

namespace Mapingway.API.Extensions.Installers;

public static class MapperInstaller
{
    public static IServiceCollection AddRequestToCommandMapper(this IServiceCollection services)
    {
        services.AddScoped<IRequestToCommandMapper, MapperlyRequestToCommandMapper>();
        services.AddScoped<IResultToResponseMapper, MapperlyResultToResponseMapper>();

        return services;
    }
}