using Rapido.Framework;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

var app = builder.Build();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.UseFramework();

app.Run();