using Rapido.Framework;
using Rapido.Framework.Health.HealthChecks;
using Rapido.Services.Wallets.Api.Endpoints;
using Rapido.Services.Wallets.Application;
using Rapido.Services.Wallets.Domain;
using Rapido.Services.Wallets.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddApplication()
    .AddDomain()
    .AddInfrastructure(builder.Configuration)
    .AddHealth(builder.Configuration);

var app = builder.Build();

app
    .MapWalletEndpoints()
    .MapOwnerEndpoints();

app.UseHealth();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app
    .MapGet("/ping", () => "pong")
    .WithTags("API")
    .WithName("Pong");

app.UseFramework();

app.Run();




public partial class Program {}