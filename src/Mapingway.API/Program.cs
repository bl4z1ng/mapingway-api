using Mapingway.API.Cors;
using Mapingway.API.Logging;
using Mapingway.API.Middleware.Exception;
using Mapingway.Application;
using Mapingway.Infrastructure;
using Mapingway.Presentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(myAllowSpecificOrigins);

builder.ConfigureLogging();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.UseCors(myAllowSpecificOrigins);
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
