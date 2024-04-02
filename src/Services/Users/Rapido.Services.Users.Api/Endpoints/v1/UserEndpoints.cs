using Microsoft.AspNetCore.Mvc;
using Rapido.Framework.CQRS.Dispatchers;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Storage;

namespace Rapido.Services.Users.Api.Endpoints.v1;

internal static class UserEndpoints
{
    private const string Version = "v1";
    
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"/{Version}/sign-up", SignUp);

        app.MapPost($"/{Version}/sign-in", SignIn);

        return app;
    }

    private static async Task<IResult> SignUp([FromBody] SignUp command, [FromServices] IDispatcher dispatcher)
    {
        var userId = Guid.NewGuid();
        await dispatcher.DispatchAsync(command with { UserId = userId });

        return Results.Ok();
    }

    private static async Task<IResult> SignIn([FromBody] SignIn command, [FromServices] IDispatcher dispatcher, 
        [FromServices] ITokenStorage tokenStorage)
    {
        await dispatcher.DispatchAsync(command);
        var token = tokenStorage.Get();

        return Results.Ok(token);
    }
}