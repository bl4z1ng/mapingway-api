using System.Reflection;
using Mapingway.API.Swagger.Documentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Configurations;

public static class SwaggerConfiguration
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        var swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly());
        services.AddSwaggerGen(options =>
        {
            options.OrderActionsBy((apiDesc) 
                => $"{swaggerControllerOrder.SortKey(apiDesc.ActionDescriptor.RouteValues["controller"])}");
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

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
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
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme 
                    {
                        Reference = new OpenApiReference 
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<Program>();

        return services;
    }
}