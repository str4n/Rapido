using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.Clients.DTO;

namespace Rapido.Services.Currencies.Tests.Integration;

internal sealed class TestExchangeRateApiClient : IExchangeRateApiClient
{
    public Task<ExchangeRatesDto> GetExchangeRates(string currency, CancellationToken cancellationToken = default)
    {
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };

        if (!currencies.Contains(currency))
        {
            throw new InvalidOperationException("Invalid currency.");
        }
        
        var result = new ExchangeRatesDto
        {
            BaseCode = currency.ToUpperInvariant()
        };

        result = currency switch
        {
            "USD" => result with { ConversionRates = GetUsdConversionRates() },
            "PLN" => result with { ConversionRates = GetPlnConversionRates() },
            "EUR" => result with { ConversionRates = GetEurConversionRates() },
            "GBP" => result with { ConversionRates = GetGbpConversionRates() },
            _ => result
        };

        return Task.FromResult(result);
    }
    
    // Helper method
    
    public static Task<ExchangeRatesDto> GetExchangeRates(string currency)
    {
        var currencies = new[] { "USD", "EUR", "PLN", "GBP" };

        if (!currencies.Contains(currency))
        {
            throw new InvalidOperationException("Invalid currency.");
        }
        
        var result = new ExchangeRatesDto
        {
            BaseCode = currency.ToUpperInvariant()
        };

        result = currency switch
        {
            "USD" => result with { ConversionRates = GetUsdConversionRates() },
            "PLN" => result with { ConversionRates = GetPlnConversionRates() },
            "EUR" => result with { ConversionRates = GetEurConversionRates() },
            "GBP" => result with { ConversionRates = GetGbpConversionRates() },
            _ => result
        };

        return Task.FromResult(result);
    }

    private static ExchangeRatesDto.ConversionRateDto GetUsdConversionRates()
        => new()
        {
            USD = 1,
            PLN = 3.95,
            EUR = 0.92,
            GBP = 0.77
        };

    private static ExchangeRatesDto.ConversionRateDto GetEurConversionRates()
        => new()
        {
            EUR = 1,
            PLN = 4.30,
            USD = 1.09,
            GBP = 0.84
        };

    private static ExchangeRatesDto.ConversionRateDto GetPlnConversionRates()
        => new()
        {
            PLN = 1,
            EUR = 0.23,
            USD = 0.25,
            GBP = 0.19
        };

    private static ExchangeRatesDto.ConversionRateDto GetGbpConversionRates()
        => new()
        {
            GBP = 1,
            PLN = 5.14,
            EUR = 1.20,
            USD = 1.30
        };
}