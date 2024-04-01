using Rapido.Services.Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore(builder.Configuration);

var app = builder.Build();

app.Run();