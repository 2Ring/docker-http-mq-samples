var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<HttpService>();
builder.Services.AddSingleton<MqService>();

var app = builder.Build();

app.MapControllers();

app.Run();