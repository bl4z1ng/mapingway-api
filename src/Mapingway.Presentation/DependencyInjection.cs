using System.Reflection;
using Mapingway.Presentation.Swagger;
using Mapingway.Presentation.v1;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.ConfigureSwagger();

        Mappings.Add();

        return services;
    }
}

public class Presentation
{
    public static readonly Assembly AssemblyReference = typeof(Presentation).Assembly;
}
