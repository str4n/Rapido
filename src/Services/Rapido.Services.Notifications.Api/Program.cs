using Rapido.Framework;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework();

var app = builder.Build();

app.UseFramework();

app.Run();