using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Messages.Commands;
using Rapido.Services.Users.Core.PasswordRecovery.Commands;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Commands;
using Rapido.Services.Users.Core.User.DTO;
using Rapido.Services.Users.Core.User.Queries;
using Rapido.Services.Users.Core.UserActivation.Commands;

namespace Rapido.Services.Users.Api.Endpoints;

internal static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/sign-up", SignUp)
            .WithTags("Account")
            .WithName("Sign up");

        app.MapPost("/sign-in", SignIn)
            .WithTags("Account")
            .WithName("Sign in");

        app.MapPut("/create-activation-token", CreateActivationToken)
            .WithTags("Account")
            .WithName("Create activation token");
        
        app.MapPut("/activate", Activate)
            .WithTags("Account")
            .WithName("Activate account");

        app.MapPut("/create-recovery-token", CreateRecoveryToken)
            .WithTags("Account")
            .WithName("Request password recovery");
        
        app.MapPut("recover-password", RecoverPassword)
            .WithTags("Account")
            .WithName("Recover password");

        app.MapGet("/me", GetMe)
            .RequireAuthorization()
            .WithTags("Account")
            .WithName("Get account");

        return app;
    }

    private static async Task<IResult> SignUp(SignUp command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();

        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command with { UserId = userId }, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> SignIn(
        SignIn command, 
        IDispatcher dispatcher, 
        ITokenStorage tokenStorage, 
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command, cancellationToken);
        var token = tokenStorage.Get();
        var user = await dispatcher.DispatchAsync(new GetUser(token.UserId), cancellationToken);

        return Results.Ok(new AuthDto(token, user.AccountType));
    }

    private static async Task<IResult> Activate(ActivateUser command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> CreateActivationToken(
        CreateActivationToken command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Created();
    }
    
    private static async Task<IResult> RecoverPassword(
        RecoverPassword command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> CreateRecoveryToken(
        CreateRecoveryToken command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            return Results.BadRequest();
        }
        
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Created();
    }

    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context, CancellationToken cancellationToken)
    {
        var userId = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetUser(userId), cancellationToken);

        return Results.Ok(result);
    }
}