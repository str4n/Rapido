using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Individual.Commands;
using Rapido.Services.Customers.Core.Individual.Queries;

namespace Rapido.Services.Customers.Api.Endpoints;

internal static class IndividualCustomerEndpoints
{
    public static IEndpointRouteBuilder MapIndividualCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapGet("/individual/me", GetMe)
            .RequireAuthorization()
            .WithTags("Individual Customer")
            .WithName("Get individual customer");
        
        app.MapPost("/individual/complete", Complete)
            .RequireAuthorization()
            .WithTags("Individual Customer")
            .WithName("Complete individual customer");
        
        app.MapPut("/individual/change-address", ChangeAddress)
            .RequireAuthorization()
            .WithTags("Individual Customer")
            .WithName("Change individual customer address");
        
        app.MapPut("/individual/change-nationality", ChangeNationality)
            .RequireAuthorization()
            .WithTags("Individual Customer")
            .WithName("Change individual customer nationality");
        
        //A backup method if for some reason the event consumer wouldn't work.
        app
            .MapPost("/customers/individual/create", Create)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Individual Customer")
            .WithName("Create individual customer")
            .WithDescription("A backup method, if for some reason the event consumer wouldn't work");

        return app;
    }
    
    private static async Task<IResult> Complete(
        CompleteIndividualCustomer command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { CustomerId = id }, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> GetMe(
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetIndividualCustomer(id), cancellationToken);
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> Create(
        CreateIndividualCustomer command, 
        IDispatcher dispatcher, 
        CancellationToken cancellationToken)
    {
        await dispatcher.DispatchAsync(command, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> ChangeAddress(
        ChangeAddress command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;
        
        await dispatcher.DispatchAsync(command with { Id =  id}, cancellationToken);

        return Results.Ok();
    }
    
    private static async Task<IResult> ChangeNationality(
        ChangeNationality command, 
        IDispatcher dispatcher, 
        IContext context, 
        CancellationToken cancellationToken)
    {
        var id = context.Identity.UserId;
        
        await dispatcher.DispatchAsync(command with { Id =  id}, cancellationToken);

        return Results.Ok();
    }
}