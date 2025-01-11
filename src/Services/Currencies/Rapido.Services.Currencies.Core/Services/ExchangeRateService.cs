using Microsoft.IdentityModel.Tokens;
using Rapido.Framework.Redis.Cache;
using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

internal sealed class ExchangeRateService(ICache cache, ExchangeRateLoader loader) : IExchangeRateService
{
    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(CancellationToken cancellationToken = default)
    {
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };
        var result = new List<ExchangeRateDto>();

        foreach (var currency in currencies)
        {
            var rates = await cache.GetAsync<List<ExchangeRateDto>>(currency, cancellationToken);

            if (rates.IsNullOrEmpty())
            {
                await loader.LoadExchangeRates(currency, cancellationToken);
                rates = await cache.GetAsync<List<ExchangeRateDto>>(currency, cancellationToken);
            }
            
            result.AddRange(rates);
        }

        return result;
    }
}