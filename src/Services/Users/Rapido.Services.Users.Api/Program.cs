using Rapido.Framework;
using Rapido.Services.Users.Api.Endpoints.v1;
using Rapido.Services.Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app.MapUserEndpoints();

app
    .MapGet("/", (AppInfo appInfo) => appInfo)
    .WithTags("API")
    .WithName("Info");

app.UseFramework();

app.Run();