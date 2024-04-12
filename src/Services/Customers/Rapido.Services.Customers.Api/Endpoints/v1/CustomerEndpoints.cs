using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Queries;

namespace Rapido.Services.Customers.Api.Endpoints.v1;

internal static class CustomerEndpoints
{
    private const string Version = "v1";
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Version}/customers", GetAll)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Get all customers");
        
        app.MapPost($"/{Version}/complete", Complete)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Complete customer");
        
        app
            .MapGet($"/{Version}/me", GetMe)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Get customer");

        app
            .MapPost(Version + "/verify/{customerId:guid}", Verify)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Verify customer");
        
        //A backup method if for some reason the event consumer wouldn't work.
        app
            .MapPost($"{Version}/customers/create", Create)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Create customer")
            .WithDescription("A backup method, if for some reason the event consumer wouldn't work");

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

    private static async Task<IResult> Verify(Guid customerId, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(new VerifyCustomer(customerId));

        return Results.Ok();
    }

    private static async Task<IResult> Create(CreateCustomer command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
    }
}