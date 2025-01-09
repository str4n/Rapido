using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Queries;

namespace Rapido.Services.Customers.Api.Endpoints;

internal static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapPost("/customers/lock/temp/{customerId:guid}", LockTemporarily)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Lock customer temporarily");
        
        app
            .MapPost("/customers/lock/perm/{customerId:guid}", LockPermanently)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Lock customer permanently");
        
        app
            .MapPost("/customers/unlock/{customerId:guid}", Unlock)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Unlock customer");

        app.MapGet("/customers/check-name/{name}", IsEmailTaken);

        return app;
    }
    
    private static async Task<IResult> LockTemporarily(
        Guid customerId, 
        LockCustomerTemporarily command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command with { CustomerId = customerId }, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> LockPermanently(
        Guid customerId, 
        LockCustomerPermanently command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command with { CustomerId = customerId }, cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> Unlock(
        Guid customerId, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(new UnlockCustomer(customerId), cancellationToken);

        return Results.Ok();
    }

    private static async Task<IResult> IsEmailTaken(string name, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new CheckNameUniqueness(name), cancellationToken);

        return Results.Ok(new { IsNameTaken = result });
    }
}