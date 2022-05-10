var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var instanceId = Guid.NewGuid();
Console.WriteLine($"Running with instance ID: {instanceId}");

app.MapGet("/{payload}", (string payload) => $"HTTP Reply from: {instanceId} - message: {payload}");

app.Run();
