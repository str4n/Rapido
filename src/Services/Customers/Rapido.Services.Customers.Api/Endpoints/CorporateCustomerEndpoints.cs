using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Customers.Application.Corporate.Commands;
using Rapido.Services.Customers.Application.Corporate.Queries;
using Rapido.Services.Customers.Application.Individual.Queries;

namespace Rapido.Services.Customers.Api.Endpoints;

internal static class CorporateCustomerEndpoints
{
    public static IEndpointRouteBuilder MapCorporateCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapGet("/corporate/me", GetMe)
            .RequireAuthorization()
            .WithTags("Corporate Customer")
            .WithName("Get corporate customer");
        
        app.MapPost("/corporate/complete", Complete)
            .RequireAuthorization()
            .WithTags("Corporate Customer")
            .WithName("Complete corporate customer");
        
        //A backup method if for some reason the event consumer wouldn't work.
        app
            .MapPost("/customers/corporate/create", Create)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Corporate Customer")
            .WithName("Create corporate customer")
            .WithDescription("A backup method, if for some reason the event consumer wouldn't work");
        
        return app;
    }
    
    private static async Task<IResult> GetMe(IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        var result = await dispatcher.DispatchAsync(new GetCorporateCustomer(id));
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> Complete(CompleteCorporateCustomer command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;

        await dispatcher.DispatchAsync(command with { CustomerId = id });

        return Results.Ok();
    }
    
    private static async Task<IResult> Create(CreateCorporateCustomer command, IDispatcher dispatcher)
    {
        await dispatcher.DispatchAsync(command);

        return Results.Ok();
    }
}