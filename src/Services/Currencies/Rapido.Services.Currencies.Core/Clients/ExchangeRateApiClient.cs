using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Rapido.Services.Currencies.Core.Clients.DTO;

namespace Rapido.Services.Currencies.Core.Clients;

internal sealed class ExchangeRateApiClient : IExchangeRateApiClient
{
    private const string BasePath = "https://v6.exchangerate-api.com/v6";
    private readonly ExchangeRateApiOptions _options;
    private readonly HttpClient _httpClient;

    public ExchangeRateApiClient(IOptions<ExchangeRateApiOptions> options, IHttpClientFactory factory)
    {
        _options = options.Value;
        _httpClient = factory.CreateClient();
    }
    
    public async Task<ExchangeRatesDto> GetExchangeRates(string currency)
    {
        var apiKey = _options.ApiKey;
        var path = $"{BasePath}/{apiKey}/latest/{currency}";

        return await _httpClient.GetFromJsonAsync<ExchangeRatesDto>(path);
    }
}