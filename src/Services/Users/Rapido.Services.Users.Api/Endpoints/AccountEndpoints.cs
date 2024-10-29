using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.DTO;
using Rapido.Services.Users.Core.Queries;
using Rapido.Services.Users.Core.Storage;

namespace Rapido.Services.Users.Api.Endpoints;

internal static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapPost("/sign-up", SignUp)
            .WithTags("Account")
            .WithName("Sign up");

        app
            .MapPost("/sign-in", SignIn)
            .WithTags("Account")
            .WithName("Sign in");;

        app
            .MapGet("/", GetMe)
            .RequireAuthorization()
            .WithTags("Account")
            .WithName("Get account");

        return app;
    }

    private static async Task<IResult> SignUp(SignUp command, IDispatcher dispatcher)
    {
        var userId = Guid.NewGuid();

        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command with { UserId = userId });

        return Results.Ok();
    }

    private static async Task<IResult> SignIn(SignIn command, IDispatcher dispatcher, ITokenStorage tokenStorage)
    {
        await dispatcher.DispatchAsync(command);
        var token = tokenStorage.Get();
        var user = await dispatcher.DispatchAsync(new GetUser(token.UserId));

        return Results.Ok(new AuthDto(token, user.AccountType));
    }

    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context)
    {
        var userId = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetUser(userId));

        return Results.Ok(result);
    }
}