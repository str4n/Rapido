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
        app.MapPost($"{Version}/complete", Complete)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Complete customer");
        
        app
            .MapGet($"/{Version}/me", GetMe)
            .RequireAuthorization()
            .WithTags("Customer")
            .WithName("Get customer");

        return app;
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
}