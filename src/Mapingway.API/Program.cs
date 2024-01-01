using Hellang.Middleware.ProblemDetails;
using Mapingway.API.Cors;
using Mapingway.API.Localization;
using Mapingway.API.Logging;
using Mapingway.Application;
using Mapingway.Infrastructure;
using Mapingway.Infrastructure.Logging;
using Mapingway.Presentation;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

builder.ConfigureLogging();

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(myAllowSpecificOrigins);

builder.Services.AddLocalizationRules();

builder.Services.AddMapster();

builder.Services.ConfigureProblemDetails(builder.Environment);

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

//TODO: exception if catched by problem details, but hot logged, need to rethrow exception in problem details and catch it in logging middleware.
// remove forming of response from this middleware, but get logger here and log the re-throwed exception
//app.UseGlobalExceptionHandling();

app
    .UseRequestLogging()
    .UseProblemDetails();

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);

app.UseRequestLocalization();

app.UseSwagger().UseSwaggerUI();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
