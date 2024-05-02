using Rapido.Framework;
using Rapido.Framework.Health.HealthChecks;
using Rapido.Services.Users.Api.Endpoints;
using Rapido.Services.Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration)
    .AddHealth(builder.Configuration);

var app = builder.Build();

app.MapAccountEndpoints();

app.MapUserEndpoints();

app.UseHealth();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.UseFramework();

app.Run();