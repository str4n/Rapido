using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Application.Wallets.Clients;

public interface ICurrencyApiClient
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates();
}