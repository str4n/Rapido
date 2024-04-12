using Rapido.Framework;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Queries;
using Rapido.Services.Users.Core.Storage;

namespace Rapido.Services.Users.Api.Endpoints.v1;

internal static class AccountEndpoints
{
    private const string Version = "v1";
    
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapPost($"/{Version}/sign-up", SignUp)
            .WithTags("Account")
            .WithName("Sign up");

        app
            .MapPost($"/{Version}/sign-in", SignIn)
            .WithTags("Account")
            .WithName("Sign in");;

        app
            .MapGet($"/{Version}/me", GetMe)
            .RequireAuthorization()
            .WithTags("Account")
            .WithName("Get account");

        return app;
    }

    private static async Task<IResult> SignUp(SignUp command, IDispatcher dispatcher)
    {
        var userId = Guid.NewGuid();
        await dispatcher.DispatchAsync(command with { UserId = userId });

        return Results.Ok();
    }

    private static async Task<IResult> SignIn(SignIn command, IDispatcher dispatcher, ITokenStorage tokenStorage)
    {
        await dispatcher.DispatchAsync(command);
        var token = tokenStorage.Get();

        return Results.Ok(token);
    }

    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context)
    {
        var userId = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetUser(userId));

        return Results.Ok(result);
    }
}