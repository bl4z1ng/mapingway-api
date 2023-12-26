using System.Reflection;
using Mapingway.Presentation.Mapping;
using Mapingway.Presentation.Swagger;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddMappers();
        services.ConfigureSwagger();

        return services;
    }
}

public class Presentation
{
    public static readonly Assembly AssemblyReference = typeof(Presentation).Assembly;
}