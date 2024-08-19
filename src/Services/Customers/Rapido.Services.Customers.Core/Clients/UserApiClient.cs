using System.Net.Http.Json;
using Rapido.Framework.Auth.ApiKeys;
using Rapido.Framework.Auth.ApiKeys.Vault;
using Rapido.Services.Customers.Core.Clients.DTO;

namespace Rapido.Services.Customers.Core.Clients;

//TODO: Extract URL to the service registry
internal sealed class UserApiClient : IUserApiClient
{
    private const string ClientName = "consul";
    private const string ApiUrl = "http://users/users";
    private readonly IHttpClientFactory _clientFactory;
    private readonly IApiKeyVault _vault;

    public UserApiClient(IHttpClientFactory clientFactory, IApiKeyVault vault)
    {
        _clientFactory = clientFactory;
        _vault = vault;
    }

    public async Task<UserDto> GetAsync(string email)
    {
        var client = _clientFactory.CreateClient(ClientName);
        var key = _vault.GetInternalKey("customers");
        
        client.DefaultRequestHeaders.Add(ApiKey.HeaderName, key);
        var result = await client.GetFromJsonAsync<UserDto>($"{ApiUrl}/{email}");

        return result;
    }
}