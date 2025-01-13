using FluentAssertions;
using MassTransit.Internals.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rapido.Framework.Testing;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.DTO;
using Rapido.Services.Currencies.Core.Services;

namespace Rapido.Services.Currencies.Tests.Integration.Services;

public class ExchangeRateLoaderTests() : ApiTests<Program>(new ApiTestOptions
{
    EnableRedis = true,
    EnablePostgres = false
})
{
    [Fact]
    public async Task verify_currency_exchange_rate_caching()
    {
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };

        using var loader = Scope.ServiceProvider.GetRequiredService<ExchangeRateLoader>();

        await loader.LoadExchangeRates();
        
        var cache = Scope.ServiceProvider.GetRequiredService<IDistributedCache>();

        foreach (var currency in currencies)
        {
            var validRates = await TestExchangeRateApiClient.GetExchangeRates(currency);

            var cachedValue = await cache.GetStringAsync(currency);

            cachedValue.Should().NotBeNull();
            
            var cachedRates =  JsonConvert.DeserializeObject<List<ExchangeRateDto>>(cachedValue!);

            cachedRates.Should().NotBeNull();
            cachedRates.Should().NotBeEmpty();

            validRates.ConversionRates.PLN
                .Should().Be(cachedRates.SingleOrDefault(x => x.From == currency && x.To == "PLN")!.Rate);
            
            validRates.ConversionRates.EUR
                .Should().Be(cachedRates.SingleOrDefault(x => x.From == currency && x.To == "EUR")!.Rate);
            
            validRates.ConversionRates.USD
                .Should().Be(cachedRates.SingleOrDefault(x => x.From == currency && x.To == "USD")!.Rate);
            
            validRates.ConversionRates.GBP
                .Should().Be(cachedRates.SingleOrDefault(x => x.From == currency && x.To == "GBP")!.Rate);
        }
    }

    #region Arrange

    protected override Action<IServiceCollection> ConfigureServices => s =>
    {
        s.AddScoped<IExchangeRateApiClient, TestExchangeRateApiClient>();
    };

    #endregion
}