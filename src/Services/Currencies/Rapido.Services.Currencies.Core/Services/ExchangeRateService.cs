using Rapido.Framework.Redis.Cache;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

internal sealed class ExchangeRateService : IExchangeRateService
{
    private readonly ICache _cache;
    private readonly ExchangeRateLoader _loader;

    public ExchangeRateService(ICache cache, ExchangeRateLoader loader)
    {
        _cache = cache;
        _loader = loader;
    }
    
    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates()
    {
        var usdRates = await _cache.GetAsync<List<ExchangeRateDto>>("USD");
        var eurRates = await _cache.GetAsync<List<ExchangeRateDto>>("EUR");
        var plnRates = await _cache.GetAsync<List<ExchangeRateDto>>("PLN");
        var gbpRates = await _cache.GetAsync<List<ExchangeRateDto>>("GBP");

        if (usdRates is null || eurRates is null || plnRates is null || gbpRates is null)
        {
            await _loader.LoadExchangeRates();
            usdRates = await _cache.GetAsync<List<ExchangeRateDto>>("USD");
            eurRates = await _cache.GetAsync<List<ExchangeRateDto>>("EUR");
            plnRates = await _cache.GetAsync<List<ExchangeRateDto>>("PLN");
            gbpRates = await _cache.GetAsync<List<ExchangeRateDto>>("GBP");
        }

        var result = new List<ExchangeRateDto>();
        
        result.AddRange(usdRates);
        result.AddRange(eurRates);
        result.AddRange(plnRates);
        result.AddRange(gbpRates);

        return result;
    }
}