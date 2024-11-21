using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Tests.Integration;

public class TestCurrencyApiClient : ICurrencyApiClient
{
    private static readonly List<ExchangeRate> ExchangeRates =
    [
        new ExchangeRate("PLN", "PLN", 1),
        new ExchangeRate("PLN", "EUR", 0.23),
        new ExchangeRate("PLN", "USD", 0.25),
        new ExchangeRate("PLN", "EUR", 0.19),

        new ExchangeRate("EUR", "EUR", 1),
        new ExchangeRate("EUR", "PLN", 4.30),
        new ExchangeRate("EUR", "USD", 1.09),
        new ExchangeRate("EUR", "GBP", 0.84),

        new ExchangeRate("USD", "USD", 1),
        new ExchangeRate("USD", "PLN", 3.95),
        new ExchangeRate("USD", "EUR", 0.92),
        new ExchangeRate("USD", "GBP", 0.77),

        new ExchangeRate("GBP", "GBP", 1),
        new ExchangeRate("GBP", "PLN", 5.14),
        new ExchangeRate("GBP", "EUR", 1.20),
        new ExchangeRate("GBP", "USD", 1.30)
    ];

    public Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        => Task.FromResult(ExchangeRates.AsEnumerable());
}