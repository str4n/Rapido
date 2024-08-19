using Rapido.Framework.Auth.ApiKeys.Filters;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Users.Core.Queries;

namespace Rapido.Services.Users.Api.Endpoints;

internal static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{email}", GetUser).AddEndpointFilter<ApiKeyEndpointFilter>();
        
        return app;
    }

    private static async Task<IResult> GetUser(string email, IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchAsync(new GetUserByEmail(email));

        return Results.Ok(result);
    }
}