using Mapingway.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "Configuration.json"));

// Add services to the container.
builder.Logging.AddFileLogger(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

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