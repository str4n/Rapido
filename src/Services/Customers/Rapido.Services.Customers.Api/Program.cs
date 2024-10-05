using Rapido.Framework;
using Rapido.Framework.Health.HealthChecks;
using Rapido.Services.Customers.Api.Endpoints;
using Rapido.Services.Customers.Application;
using Rapido.Services.Customers.Domain;
using Rapido.Services.Customers.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddApplication()
    .AddDomain()
    .AddInfrastructure(builder.Configuration)
    .AddHealth(builder.Configuration);

var app = builder.Build();

app
    .MapCustomerEndpoints()
    .MapIndividualCustomerEndpoints();

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