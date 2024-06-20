using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Queries;

namespace Rapido.Services.Customers.Api.Endpoints;

internal static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/customers", GetAll)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Get all customers");
        
        app.MapPost("/complete", Complete)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Complete customer");
        
        app
            .MapGet("/me", GetMe)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Get customer");
        
        //A backup method if for some reason the event consumer wouldn't work.
        app
            .MapPost("/customers/create", Create)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Create customer")
            .WithDescription("A backup method, if for some reason the event consumer wouldn't work");

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

    private static async Task<IResult> GetAll(IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchAsync(new GetCustomers());

        return Results.Ok(result);
    }

    private static async Task<IResult> Complete(CompleteCustomer command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { CustomerId = id });

        return Results.Ok();
    }

    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetCustomer(id));
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> Create(CreateCustomer command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
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