using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Services.Users.Core.Queries;

namespace Rapido.Services.Users.Api.Endpoints.v1;

internal static class UserEndpoints
{
    private const string Version = "v1";
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(Version + "/users/{email}", GetUser);
        
        return app;
    }

    private static async Task<IResult> GetUser(string email, IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchAsync(new GetUserByEmail(email));

        return Results.Ok(result);
    }
}