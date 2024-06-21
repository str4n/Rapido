using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rapido.Framework.Redis.Cache;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

internal sealed class ExchangeRateLoader : IHostedService, IDisposable
{
    private readonly IExchangeRateApiClient _client;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public ExchangeRateLoader(IExchangeRateApiClient client, IServiceProvider serviceProvider)
    {
        _client = client;
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));
    }

    public async void DoWork(object state)
    {
        await LoadExchangeRates();
    }

    public async Task LoadExchangeRates()
    {
        const string usd = "USD";
        const string eur = "EUR";
        const string pln = "PLN";
        const string gbp = "GBP";

        using var scope = _serviceProvider.CreateScope();

        var cache = scope.ServiceProvider.GetRequiredService<ICache>();
        
        var usdRates = await _client.GetExchangeRates(usd);
        var eurRates = await _client.GetExchangeRates(eur);
        var plnRates = await _client.GetExchangeRates(pln);
        var gbpRates = await _client.GetExchangeRates(gbp);

        var usdDtos = new List<ExchangeRateDto>
        {
            new(usd,usd, usdRates.ConversionRates.USD),
            new(usd,eur, usdRates.ConversionRates.EUR),
            new(usd,pln, usdRates.ConversionRates.PLN),
            new(usd,gbp, usdRates.ConversionRates.GBP),
        };
        
        var eurDtos = new List<ExchangeRateDto>
        {
            new(eur,usd, eurRates.ConversionRates.USD),
            new(eur,eur, eurRates.ConversionRates.EUR),
            new(eur,pln, eurRates.ConversionRates.PLN),
            new(eur,gbp, eurRates.ConversionRates.GBP),
        };
        
        var plnDtos = new List<ExchangeRateDto>
        {
            new(pln,usd, plnRates.ConversionRates.USD),
            new(pln,eur, plnRates.ConversionRates.EUR),
            new(pln,pln, plnRates.ConversionRates.PLN),
            new(pln,gbp, plnRates.ConversionRates.GBP),
        };
        
        var gbpDtos = new List<ExchangeRateDto>
        {
            new(gbp,usd, gbpRates.ConversionRates.USD),
            new(gbp,eur, gbpRates.ConversionRates.EUR),
            new(gbp,pln, gbpRates.ConversionRates.PLN),
            new(gbp,gbp, gbpRates.ConversionRates.GBP),
        };

        await cache.SetAsync(usd, usdDtos);
        await cache.SetAsync(eur, eurDtos);
        await cache.SetAsync(pln, plnDtos);
        await cache.SetAsync(gbp, gbpDtos);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public void Dispose()
    {
        _timer?.Dispose();
    }
}