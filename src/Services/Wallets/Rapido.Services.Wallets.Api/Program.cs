using Rapido.Framework;
using Rapido.Services.Wallets.Application;
using Rapido.Services.Wallets.Domain;
using Rapido.Services.Wallets.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddApplication()
    .AddDomain()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.Run();