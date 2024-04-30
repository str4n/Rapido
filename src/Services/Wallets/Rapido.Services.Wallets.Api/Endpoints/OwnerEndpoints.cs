using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Wallets.Application.Owners.Commands;
using Rapido.Services.Wallets.Application.Owners.Queries;

namespace Rapido.Services.Wallets.Api.Endpoints;

internal static class OwnerEndpoints
{
    public static IEndpointRouteBuilder MapOwnerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/transform", TransformOwner)
            .RequireAuthorization()
            .WithTags("Owners")
            .WithName("Transform individual owner into corporate owner");

        app.MapGet("/owners/individual", GetAllIndividualOwners)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Owners")
            .WithName("Get all individual owners");
        
        app.MapGet("/owners/corporate", GetAllCorporateOwners)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Owners")
            .WithName("Get all corporate owners");
        
        return app;
    }

    private static async Task<IResult> TransformOwner(TransformOwnerIntoCorporate command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;
        await dispatcher.DispatchAsync(command with { OwnerId = id });

        return Results.Ok();
    }

    private static async Task<IResult> GetAllIndividualOwners(IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchAsync(new GetIndividualOwners());

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetAllCorporateOwners(IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchAsync(new GetCorporateOwners());

        return Results.Ok(result);
    }
}