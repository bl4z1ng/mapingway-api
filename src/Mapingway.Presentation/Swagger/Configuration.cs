using Mapingway.Presentation.Swagger.Filters.Document;
using Mapingway.Presentation.Swagger.Filters.Operation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger;

public static class Configuration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mapingway API",
                Description = "An ASP.NET Core Web API for managing routes and checkpoints of Mapingway Web Cient",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Name = "Max Pyte",
                    Email = "without.auth0@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/max-pyte/")
                }
            });

            var xmlFilename = $"{Presentation.AssemblyReference.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.OperationFilter<SwaggerLocalizationFilter>();
            options.EnableAnnotations();
            options.ExampleFilters();
            options.AddCommonStatusCodesResponses();

            options.ConvertRoutesToCamelCase();
        });

        services.AddSwaggerExamplesFromAssemblyOf<Presentation>();

        return services;
    }
}
