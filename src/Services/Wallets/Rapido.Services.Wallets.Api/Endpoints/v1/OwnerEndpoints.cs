using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Services.Wallets.Application.Owners.Commands;

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
        
        return app;
    }

    private static async Task<IResult> TransformOwner(TransformOwnerToCorporate command, IDispatcher dispatcher, IContext context)
    {
        var id = context.Identity.UserId;
        await dispatcher.DispatchAsync(command with { OwnerId = id });

        return Results.Ok();
    }
}