using Rapido.Framework;
using Rapido.Framework.Auth.ApiKeys.Filters;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Notifications.Core;
using Rapido.Services.Notifications.Core.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app.MapPost("/emails/send", async (SendEmail command, IDispatcher dispatcher) =>
{
    await dispatcher.DispatchAsync(command);
    return Results.NoContent();
}).AddEndpointFilter<ApiKeyEndpointFilter>();

app.UseFramework();

app.Run();