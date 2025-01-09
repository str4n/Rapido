using Rapido.Services.Currencies.Core.Clients.DTO;

namespace Rapido.Services.Currencies.Core.Clients;

internal interface IExchangeRateApiClient
{
    Task<ExchangeRatesDto> GetExchangeRates(string currency, CancellationToken cancellationToken = default);
}