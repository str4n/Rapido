using Rapido.Framework;
using Rapido.Saga;
using Rapido.Saga.Persistence;
using Rapido.Saga.Sagas;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.AddSagas();

var app = builder.Build();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.UseFramework();

app.Run();