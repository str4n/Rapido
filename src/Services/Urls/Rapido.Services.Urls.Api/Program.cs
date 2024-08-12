using Rapido.Framework;
using Rapido.Services.Urls.Core;
using Rapido.Services.Urls.Core.Requests;
using Rapido.Services.Urls.Core.Services;

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

app.MapPost("/", async (ShortenUrl request, IUrlShortenerService service) =>
{
    await service.ShortenUrl(request);

    return Results.NoContent();
});

app.MapGet("{alias}", async (string alias, IUrlShortenerService service) =>
{
    var url = await service.GetRedirection(alias);

    return Results.Redirect(url);
});

app.UseFramework();

app.Run();