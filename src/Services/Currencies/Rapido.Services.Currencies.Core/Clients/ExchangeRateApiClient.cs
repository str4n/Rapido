﻿using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Rapido.Framework.Auth.ApiKeys.Vault;
using Rapido.Services.Currencies.Core.Clients.DTO;

namespace Rapido.Services.Currencies.Core.Clients;

internal sealed class ExchangeRateApiClient : IExchangeRateApiClient
{
    private const string BasePath = "https://v6.exchangerate-api.com/v6";
    private readonly IApiKeyVault _vault;
    private readonly HttpClient _httpClient;

    public ExchangeRateApiClient(IHttpClientFactory factory, IApiKeyVault vault)
    {
        _vault = vault;
        _httpClient = factory.CreateClient();
    }
    
    public async Task<ExchangeRatesDto> GetExchangeRates(string currency)
    {
        var apiKey = _vault.GetExternalKey("exchangeRate");
        var path = $"{BasePath}/{apiKey}/latest/{currency}";

        return await _httpClient.GetFromJsonAsync<ExchangeRatesDto>(path);
    }
}