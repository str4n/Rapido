using Rapido.Framework;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Urls.Core;
using Rapido.Services.Urls.Core.Commands;
using Rapido.Services.Urls.Core.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app
    .MapGet("/ping", () => "pong")
    .WithTags("API")
    .WithName("Pong");

app.MapPost("/", async (ShortenUrl command, IDispatcher dispatcher) =>
{
    await dispatcher.DispatchAsync(command);

    return Results.NoContent();
});

app.MapGet("/link/{alias}", async (string alias, IDispatcher dispatcher) =>
{
    var url = await dispatcher.DispatchAsync(new GetRedirection(alias));

    return Results.Redirect(url);
});

app.UseFramework();

app.Run();