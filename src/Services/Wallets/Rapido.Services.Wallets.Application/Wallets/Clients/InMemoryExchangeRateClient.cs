using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Application.Wallets.Clients;

internal sealed class InMemoryExchangeRateClient : IExchangeRateApiClient
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        => Task.FromResult(new List<ExchangeRate>
        {
            new ExchangeRate("PLN", "EUR", 0.23),
            new ExchangeRate("EUR", "PLN", 4.37),
            new ExchangeRate("PLN", "PLN", 1),
            new ExchangeRate("EUR", "EUR", 1),
        }.AsEnumerable());
}