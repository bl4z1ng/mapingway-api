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
            options.OperationFilter<SwaggerLocalizationFilter>();

            var xmlFilename = $"{Presentation.AssemblyReference.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer " +
                              "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                              ".eyJzdWIiOiJTdXBlckppamEifQ" +
                              ".FxuEPRdkBjpbq_3bH0cdwTUqvXpMA7IceNNC3ZgYIzId\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            options.EnableAnnotations();
            options.ExampleFilters();
            options.AddCommonStatusCodesResponses();

            options.ConvertRoutesToCamelCase();
        });

        services.AddSwaggerExamplesFromAssemblyOf<Presentation>();

        return services;
    }
}
