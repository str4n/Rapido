using Rapido.Framework;
using Rapido.Services.Currencies.Api.Endpoints;
using Rapido.Services.Currencies.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app.MapCurrencyEndpoints();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");


app.UseFramework();

app.Run();






public partial class Program {}