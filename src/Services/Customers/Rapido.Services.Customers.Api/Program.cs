using Rapido.Framework;
using Rapido.Framework.Health.HealthChecks;
using Rapido.Services.Customers.Api.Endpoints;
using Rapido.Services.Customers.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration)
    .AddHealth(builder.Configuration);

var app = builder.Build();

app
    .MapCustomerEndpoints()
    .MapIndividualCustomerEndpoints()
    .MapCorporateCustomerEndpoints();

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