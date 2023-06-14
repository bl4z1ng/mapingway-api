using Mapingway.API.Extensions;
using Mapingway.Application;
using Mapingway.Application.Users.Interfaces;
using Mapingway.Common.Options;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine("Configuration", "Configuration.json"), optional: false, reloadOnChange: true);

builder.Logging.AddFileLogger(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(DbOptions.Development));
    builder.Services.AddDbContext<ApplicationDbContext, DevelopmentDbContext>();
}
else
{
    builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(DbOptions.Production));
    builder.Services.AddDbContext<ApplicationDbContext>();
}

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(ApplicationAssembly.AssemblyReference);
});

builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();