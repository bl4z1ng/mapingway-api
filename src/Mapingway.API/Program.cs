using Mapingway.API.Configurations;
using Mapingway.API.Installers;
using Mapingway.Application;
using Mapingway.Infrastructure.Authentication.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var  myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: myAllowSpecificOrigins, policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("Configuration.json", optional: false, reloadOnChange: true);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.ConfigureDbContext();

builder.Services.AddControllers();

builder.Services.AddMappers();

builder.Services.ConfigureSwagger();

// Infrastructure.
builder.Services.AddRepositoriesAndUnitOfWork();

builder.Services.AddAuthenticationServices();

builder.ConfigureHashing();

// Application.
builder.ConfigureValidationBehavior();
builder.ConfigureLoggingBehavior();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Application.AssemblyReference);
});

// Authentication and authorization configuration.
builder.ConfigureJwt();
builder.Services
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

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    
//    // if need dark theme (instead of UseSwaggerUI())
//    // app.UseSwaggerUIDark();
//}
app.UseCors(myAllowSpecificOrigins);
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();