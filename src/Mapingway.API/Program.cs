using Hellang.Middleware.ProblemDetails;
using Mapingway.API.Cors;
using Mapingway.API.Localization;
using Mapingway.Application;
using Mapingway.Infrastructure;
using Mapingway.Infrastructure.Logging;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.Presentation;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog<Program>(opt =>
{
    opt.MinimumLogLevel = builder.Environment.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Information;
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
