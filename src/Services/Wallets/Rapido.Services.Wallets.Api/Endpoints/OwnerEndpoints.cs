using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Wallets.Application.Owners.Queries;

namespace Rapido.Services.Wallets.Api.Endpoints;

internal static class OwnerEndpoints
{
    public static IEndpointRouteBuilder MapOwnerEndpoints(this IEndpointRouteBuilder app)
    {
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

    private static async Task<IResult> GetAllIndividualOwners(IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new GetIndividualOwners(), cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetAllCorporateOwners(IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new GetCorporateOwners(), cancellationToken);

        return Results.Ok(result);
    }
}