using Rapido.Framework.Redis.Cache;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

internal sealed class ExchangeRateService(ICache cache, ExchangeRateLoader loader) : IExchangeRateService
{
    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(CancellationToken cancellationToken = default)
    {
        var usdRates = await cache.GetAsync<List<ExchangeRateDto>>("USD", cancellationToken);
        var eurRates = await cache.GetAsync<List<ExchangeRateDto>>("EUR", cancellationToken);
        var plnRates = await cache.GetAsync<List<ExchangeRateDto>>("PLN", cancellationToken);
        var gbpRates = await cache.GetAsync<List<ExchangeRateDto>>("GBP", cancellationToken);

        if (usdRates is null || eurRates is null || plnRates is null || gbpRates is null)
        {
            await loader.LoadExchangeRates(cancellationToken);
            usdRates = await cache.GetAsync<List<ExchangeRateDto>>("USD", cancellationToken);
            eurRates = await cache.GetAsync<List<ExchangeRateDto>>("EUR", cancellationToken);
            plnRates = await cache.GetAsync<List<ExchangeRateDto>>("PLN", cancellationToken);
            gbpRates = await cache.GetAsync<List<ExchangeRateDto>>("GBP", cancellationToken);
        }

        var result = new List<ExchangeRateDto>();
        
        result.AddRange(usdRates);
        result.AddRange(eurRates);
        result.AddRange(plnRates);
        result.AddRange(gbpRates);

        return result;
    }
}