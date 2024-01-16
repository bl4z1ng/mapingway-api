using Hellang.Middleware.ProblemDetails;
using Mapingway.API.Cors;
using Mapingway.API.Localization;
using Mapingway.Application;
using Mapingway.Infrastructure;
using Mapingway.Infrastructure.Logging;
using Mapingway.Infrastructure.Logging.CorrelationToken;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.Presentation;
using Mapster;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog(clearDefaultProviders: true);
builder.Services.ConfigureProblemDetails(builder.Environment);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(myAllowSpecificOrigins);

builder.Services.AddLocalizationRules();

builder.Services.AddMapster();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Environment);

var app = builder.Build();

app.UseCorrelationToken();
app.UseRequestLogging();
app.UseProblemDetails();

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);

app.UseRequestLocalization();

app.UseSwagger().UseSwaggerUI();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
