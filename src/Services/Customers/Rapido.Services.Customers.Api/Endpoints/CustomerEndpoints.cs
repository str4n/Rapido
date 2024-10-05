using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Customers.Application.Common.Commands;

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

        return app;
    }
    
    private static async Task<IResult> LockTemporarily(Guid customerId, LockCustomerTemporarily command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command with { CustomerId = customerId });

        return Results.Ok();
    }
    
    private static async Task<IResult> LockPermanently(Guid customerId, LockCustomerPermanently command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command with { CustomerId = customerId });

        return Results.Ok();
    }

    private static async Task<IResult> Unlock(Guid customerId, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(new UnlockCustomer(customerId));

        return Results.Ok();
    }
}