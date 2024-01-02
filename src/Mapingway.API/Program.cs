using Hellang.Middleware.ProblemDetails;
using Mapingway.API.Cors;
using Mapingway.API.Localization;
using Mapingway.Application;
using Mapingway.Infrastructure;
using Mapingway.Infrastructure.Logging;
using Mapingway.Infrastructure.ProblemDetails;
using Mapingway.Presentation;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

builder.Host.UseSerilog<Program>(opt =>
{
    opt.ClearProviders = true;
    opt.MinimumLogLevel = builder.Environment.IsProduction()
        ? LogEventLevel.Warning
        : LogEventLevel.Information;
});
builder.Services.ConfigureProblemDetails(builder.Environment);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(myAllowSpecificOrigins);

builder.Services.AddLocalizationRules();

builder.Services.AddMapster();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

// exception if catched by problem details, but hot logged, need to rethrow exception in problem details and catch it in logging middleware.
// remove forming of response from this middleware, but get logger here and log the re-throwed exception
//app.UseGlobalExceptionHandling();

app.UseRequestLoggingWith<ProblemDetailsMiddleware>();

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);

app.UseRequestLocalization();

app.UseSwagger().UseSwaggerUI();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
