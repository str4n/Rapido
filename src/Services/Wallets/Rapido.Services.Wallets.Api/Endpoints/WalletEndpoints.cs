using Microsoft.AspNetCore.Mvc;
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
        app.MapPost("/transfer/id", TransferFundsByWalletId)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Transfer funds by wallet id");
        
        app.MapPost("/transfer/name", TransferFundsByReceiverName)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Transfer receiver name");

        app.MapPut("/add-funds", AddFunds)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Wallets")
            .WithName("Add funds");

        app.MapPut("/deduct-funds", DeductFunds)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Wallets")
            .WithName("Deduct funds");

        app.MapGet("/wallet", GetWallet)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Get owner wallet");

        app.MapPost("/add-balance", AddBalance)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Add balance to wallet");
        
        //Endpoint for frontend validator
        
        app.MapGet("/has-sufficient-funds", HasSufficientFunds)
            .RequireAuthorization()
            .WithTags("Wallets")
            .WithName("Check if wallet has enough funds");

        return app;
    }

    private static async Task<IResult> TransferFundsByWalletId(
        TransferFundsByWalletId command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { OwnerId = id }, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> TransferFundsByReceiverName(
        TransferFundsByReceiverName command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { OwnerId = id }, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> AddFunds(
        AddFunds command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> DeductFunds(
        DeductFunds command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> GetWallet(
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetWallet(id), cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> AddBalance(
        AddBalance command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { OwnerId = id }, cancellationToken);

        return Results.Created();
    }

    private static async Task<IResult> HasSufficientFunds(
        [FromQuery] double amount, 
        [FromQuery] string currency, 
        [FromServices]IDispatcher dispatcher, 
        [FromServices]IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        var query = new CheckSufficiencyOfFunds(id, amount, currency);
        
        var result = await dispatcher.DispatchAsync(query with { OwnerId = id }, cancellationToken);

        return Results.Ok(result);
    }
}