using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rapido.Framework.Redis.Cache;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.Clients.DTO;
using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

internal sealed class ExchangeRateLoader(IExchangeRateApiClient client, IServiceProvider serviceProvider)
    : IHostedService, IDisposable
{
    private const int CacheLifetime = 1800; // In seconds
    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        await LoadExchangeRates();
    }
    
    // Load for single currency
    public async Task LoadExchangeRates(string currency, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();

        var cache = scope.ServiceProvider.GetRequiredService<ICache>();
        
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };
        
        var rates = await client.GetExchangeRates(currency, cancellationToken);

        var dtos = currencies
            .Select(targetCurrency => new ExchangeRateDto(currency, targetCurrency, GetRateByReflection(targetCurrency, rates)))
            .ToList();

        await cache.SetAsync(currency, dtos, TimeSpan.FromSeconds(CacheLifetime), cancellationToken);
    }
    
    // Load for all currencies
    private async Task LoadExchangeRates(CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();

        var cache = scope.ServiceProvider.GetRequiredService<ICache>();
        
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };
        
        foreach (var currency in currencies)
        {
            var rates = await client.GetExchangeRates(currency, cancellationToken);

            var dtos = currencies
                .Select(targetCurrency => new ExchangeRateDto(currency, targetCurrency, GetRateByReflection(targetCurrency, rates)))
                .ToList();

            await cache.SetAsync(currency, dtos,TimeSpan.FromSeconds(CacheLifetime), cancellationToken);
        }
    }
    
    private static double GetRateByReflection(string targetCurrency, ExchangeRatesDto rates)
        => rates.ConversionRates
            .GetType()
            .GetProperty(targetCurrency)?
            .GetValue(rates.ConversionRates, null) as double? ?? 0;

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public void Dispose()
    {
        _timer?.Dispose();
    }
}