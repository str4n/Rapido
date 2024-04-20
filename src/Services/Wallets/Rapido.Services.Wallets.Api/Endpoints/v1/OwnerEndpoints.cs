using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Wallets.Application.Owners.Commands;
using Rapido.Services.Wallets.Application.Owners.Queries;

namespace Rapido.Services.Wallets.Api.Endpoints.v1;

internal static class OwnerEndpoints
{
    private const string Version = "v1";

    public static IEndpointRouteBuilder MapOwnerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{Version}/transform", TransformOwner)
            .RequireAuthorization()
            .WithTags("Owners")
            .WithName("Transform individual owner to corporate owner");

        app.MapGet($"{Version}/owners/individual", GetAllIndividualOwners)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Owners")
            .WithName("Get all individual owners");
        
        app.MapGet($"{Version}/owners/corporate", GetAllCorporateOwners)
            .RequireAuthorization(Policies.Admin)
            .WithTags("Owners")
            .WithName("Get all corporate owners");
        
        return app;
    }

    private static async Task<IResult> TransformOwner(TransformOwnerToCorporate command, IDispatcher dispatcher, IContext context)
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