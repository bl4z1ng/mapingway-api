using System.Net.Mime;
using Mapingway.API.Extensions;
using Mapingway.API.OptionsSetup;
using Mapingway.Application;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Authentication.Permission;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Persistence.Options;
using Mapingway.Infrastructure.Persistence.Repositories;
using Mapingway.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

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
    builder.Services.AddDbContext<ApplicationDbContext, DevelopmentDbContext>();
}
else
{
    builder.Services.Configure<DbOptions>(
        builder.Configuration.GetSection(DbOptions.ProductionConfigurationSection));
    builder.Services.AddDbContext<ApplicationDbContext>();
}

builder.Services.AddControllers();

builder.Services.ConfigureSwagger();

// Infrastructure services registration.
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.ConfigureOptions<HashOptionsSetup>();

// Application services registration.
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(ApplicationAssembly.AssemblyReference);
});

// Authentication and authorization configuration.
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<TokenValidationParametersSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // if need dark theme
    // app.UseStaticFiles();
    // app.UseSwaggerUI(options =>
    // {
    //     options.InjectStylesheet("/swagger-dark.css");
    // });
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();