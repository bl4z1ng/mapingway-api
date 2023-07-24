using Mapingway.Application.Behaviors;
using MediatR;

namespace Mapingway.API.Extensions.Configuration;

public static class LoggingConfiguration
{
    public static WebApplicationBuilder ConfigureLoggingBehavior(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(
            typeof(IPipelineBehavior<,>), 
            typeof(LoggingPipelineBehavior<,>));
        
        return builder;
    }
}