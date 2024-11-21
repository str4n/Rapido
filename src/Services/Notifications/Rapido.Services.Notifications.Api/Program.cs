using Rapido.Framework;
using Rapido.Services.Notifications.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app.UseFramework();

app.Run();