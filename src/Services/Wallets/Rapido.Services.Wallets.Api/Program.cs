using Rapido.Framework;
using Rapido.Services.Wallets.Api.Endpoints.v1;
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

app.MapOwnerEndpoints();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.UseFramework();

app.Run();