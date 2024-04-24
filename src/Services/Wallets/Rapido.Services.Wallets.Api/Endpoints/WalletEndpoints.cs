using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Wallets.Application.Wallets.Commands;
using Rapido.Services.Wallets.Application.Wallets.Queries;

namespace Rapido.Services.Wallets.Api.Endpoints;

internal static class WalletEndpoints
{
    public static IEndpointRouteBuilder MapWalletEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/transfer", TransferFunds)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Transfer funds");

        app.MapPut("/add-funds", AddFunds)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Wallets")
            .WithName("Add funds");

        app.MapPut("/deduct-funds", DeductFunds)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Wallets")
            .WithName("Deduct funds");

        app.MapGet("/wallets", GetWallets)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Get owner wallets");

        app.MapPost("/create", CreateWallet)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Create wallet");

        return app;
    }

    private static async Task<IResult> TransferFunds(TransferFunds command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { OwnerId = id });

        return Results.Ok();
    }

    private static async Task<IResult> AddFunds(AddFunds command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
    }

    private static async Task<IResult> DeductFunds(DeductFunds command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
    }

    private static async Task<IResult> GetWallets(IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetOwnerWallets(id));

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateWallet(CreateWallet command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;
        
        await dispatcher.DispatchAsync(command with { OwnerId = id });

        return Results.Created();
    }
}