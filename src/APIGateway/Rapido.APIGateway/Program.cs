using Rapido.Framework;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("reverseProxy"));

var app = builder.Build();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app
    .MapGet("/ping", () => "pong")
    .WithTags("API")
    .WithName("Pong");

app.UseFramework();

app.MapReverseProxy();

app.Run();