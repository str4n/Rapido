using Rapido.Framework;
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
});

app.UseFramework();

app.Run();