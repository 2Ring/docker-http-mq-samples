var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<HttpService>();

var app = builder.Build();

app.MapControllers();

app.Run();