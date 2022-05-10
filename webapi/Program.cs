var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<HttpService>();
builder.Services.AddSingleton<MqService>();

var app = builder.Build();

app.MapControllers();

app.Run();