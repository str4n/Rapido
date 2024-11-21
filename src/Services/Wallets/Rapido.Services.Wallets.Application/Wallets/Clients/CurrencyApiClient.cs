using System.Collections;
using System.Net.Http.Json;
using Rapido.Framework.Auth.ApiKeys;
using Rapido.Framework.Auth.ApiKeys.Vault;
using Rapido.Services.Wallets.Application.Wallets.Clients.DTO;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Application.Wallets.Clients;

internal sealed class CurrencyApiClient : ICurrencyApiClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IApiKeyVault _vault;
    private const string ClientName = "consul";
    private const string ApiUrl = "http://currencies-service/rates";
    
    public CurrencyApiClient(IHttpClientFactory clientFactory, IApiKeyVault vault)
    {
        _clientFactory = clientFactory;
        _vault = vault;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        var client = _clientFactory.CreateClient(ClientName);
        var key = _vault.GetInternalKey("wallets");
        
        client.DefaultRequestHeaders.Add(ApiKey.HeaderName, key);

        var result = await client.GetFromJsonAsync<IEnumerable<ExchangeRateDto>>(ApiUrl);

        return result.Select(x => new ExchangeRate(x.From, x.To, x.Rate));
    }
}