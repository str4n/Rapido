using System.Net.Http.Json;
using Rapido.Services.Wallets.Application.Wallets.Clients.DTO;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Application.Wallets.Clients;

internal sealed class CurrencyApiClient : ICurrencyApiClient
{
    private const string ClientName = "consul";
    private const string ApiUrl = "http://currencies/rates";
    private readonly HttpClient _client;
    
    public CurrencyApiClient(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient(ClientName);
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<ExchangeRateDto>>(ApiUrl);

        return result.Select(x => new ExchangeRate(x.From, x.To, x.Rate));
    }
}