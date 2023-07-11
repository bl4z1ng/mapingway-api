using System.Text.Json.Serialization;
using Mapingway.API.Extensions.Configuration;
using Mapingway.API.Extensions.Installers;
using Mapingway.API.Internal.Mapping;
using Mapingway.Application;
using Mapingway.Infrastructure.Authentication.Permission;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    Path.Combine("Configuration", "Configuration.json"), 
    optional: false, 
    reloadOnChange: true);

builder.Logging.AddFileLogger(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<DbOptions>(
        builder.Configuration.GetSection(DbOptions.DevelopmentConfigurationSection));
    builder.Services.AddDbContext<DbContext, DevelopmentDbContext>();
}
else
{
    builder.Services.Configure<DbOptions>(
        builder.Configuration.GetSection(DbOptions.ProductionConfigurationSection));
    builder.Services.AddDbContext<DbContext, ApplicationDbContext>();
}

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped<IMapper, MapperlyMapper>();

builder.Services.ConfigureSwagger();

// Infrastructure.
builder.Services.AddRepositoriesAndUnitOfWork();

builder.Services.AddAuthenticationService();

builder.Services.ConfigureHashing();

// Application.
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(ApplicationAssembly.AssemblyReference);
});

// Authentication and authorization configuration.
builder.Services
    .ConfigureJwt()
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer();

builder.Services
    .AddAuthorization()
    .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
    .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // if need dark theme
    // app.UseSwaggerDarkTheme();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();