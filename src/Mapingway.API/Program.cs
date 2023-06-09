using Mapingway.API.Extensions;
using Mapingway.Common.Options;
using Mapingway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "Configuration.json"));

// Add services to the container.
builder.Logging.AddFileLogger(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

if (builder.Environment.IsDevelopment())
{
    services.Configure<DatabaseConfigurationOptions>(
        configuration.GetSection("DevelopmentDatabaseConfiguration"));
    services.AddDbContext<ApplicationDbContext, DevelopmentDbContext>();
}
else
{
    services.Configure<DatabaseConfigurationOptions>(
        configuration.GetSection("DatabaseConfiguration"));
    services.AddDbContext<ApplicationDbContext>();
}

services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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