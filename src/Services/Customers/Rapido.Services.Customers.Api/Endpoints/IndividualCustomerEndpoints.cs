using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Customers.Application.Individual.Commands;
using Rapido.Services.Customers.Application.Individual.Queries;

namespace Rapido.Services.Customers.Api.Endpoints;

internal static class IndividualCustomerEndpoints
{
    public static IEndpointRouteBuilder MapIndividualCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapGet("/individual/me", GetMe)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Get customer");
        
        app.MapPost("/individual/complete", Complete)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Complete customer");
        
        //A backup method if for some reason the event consumer wouldn't work.
        app
            .MapPost("/customers/individual/create", Create)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Customer")
            .WithName("Create customer")
            .WithDescription("A backup method, if for some reason the event consumer wouldn't work");

        return app;
    }
    
    private static async Task<IResult> Complete(CompleteIndividualCustomer command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { CustomerId = id });

        return Results.Ok();
    }
    
    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetIndividualCustomer(id));
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> Create(CreateIndividualCustomer command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
    }
}