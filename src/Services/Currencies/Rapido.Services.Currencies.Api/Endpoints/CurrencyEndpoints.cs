using Microsoft.AspNetCore.Http.HttpResults;
using Rapido.Framework.Auth.ApiKeys.Attributes;
using Rapido.Framework.Auth.ApiKeys.Filters;
using Rapido.Services.Currencies.Core.Services;

namespace Rapido.Services.Currencies.Api.Endpoints;

internal static class CurrencyEndpoints
{
    public static IEndpointRouteBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/rates", GetExchangeRates).AddEndpointFilter<ApiKeyEndpointFilter>();
        
        return app;
    }
    
    private static async Task<IResult> GetExchangeRates(IExchangeRateService service, CancellationToken cancellationToken)
    {
        var result = await service.GetExchangeRates(cancellationToken);

        return Results.Ok(result);
    }
}