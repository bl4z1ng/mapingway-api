using Mapingway.API.Extensions;
using Mapingway.Common.Options;
using Mapingway.Infrastructure;
using Mapingway.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "Configuration.json"));

// Add services to the container.
builder.Logging.AddFileLogger(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

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