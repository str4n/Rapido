using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Rapido.Framework.Auth.ApiKeys.Vault;
using Rapido.Services.Currencies.Core.Clients.DTO;

namespace Rapido.Services.Currencies.Core.Clients;

internal sealed class ExchangeRateApiClient(IHttpClientFactory factory, IApiKeyVault vault) : IExchangeRateApiClient
{
    private const string BasePath = "https://v6.exchangerate-api.com/v6";

    public async Task<ExchangeRatesDto> GetExchangeRates(string currency, CancellationToken cancellationToken = default)
    {
        var client = factory.CreateClient();
        var apiKey = vault.GetExternalKey("exchangeRate");
        var path = $"{BasePath}/{apiKey}/latest/{currency}";

        return await client.GetFromJsonAsync<ExchangeRatesDto>(path, cancellationToken);
    }
}