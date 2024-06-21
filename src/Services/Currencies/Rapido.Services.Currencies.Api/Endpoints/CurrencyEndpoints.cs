using Microsoft.AspNetCore.Http.HttpResults;
using Rapido.Services.Currencies.Core.Services;

namespace Rapido.Services.Currencies.Api.Endpoints;

internal static class CurrencyEndpoints
{
    public static IEndpointRouteBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/rates", GetExchangeRates);
        
        return app;
    }

    private static async Task<IResult> GetExchangeRates(IExchangeRateService service)
    {
        var result = await service.GetExchangeRates();

        return Results.Ok(result);
    }
}