using System.Net.Http.Json;
using Rapido.Services.Notifications.Core.Clients.DTO;
using Rapido.Services.Notifications.Core.Clients.Requests;

namespace Rapido.Services.Notifications.Core.Clients;

internal sealed class UrlShortenerApiClient : IUrlShortenerApiClient
{
    private const string ClientName = "consul";
    private const string ApiUrl = "http://urls/";

    private const string DefaultScheme = "https";
    private const string DefaultHost = "localhost";
    private readonly IHttpClientFactory _clientFactory;
    
    public UrlShortenerApiClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    public async Task<ShortenedUrlDto> ShortenUrl(string url)
    {
        var client = _clientFactory.CreateClient(ClientName);
        
        var result = await client.PostAsJsonAsync(ApiUrl, new ShortenUrl(DefaultScheme, DefaultHost, url));

        var content = await result.Content.ReadFromJsonAsync<ShortenedUrlDto>();

        return content;
    }
}