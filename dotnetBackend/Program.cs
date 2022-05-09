var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var instanceName = Guid.NewGuid();

app.MapPost("/", () => $"Instance name: {instanceName}");

app.Run();
