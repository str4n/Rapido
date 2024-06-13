using System.Net.Http.Json;
using Rapido.Services.Customers.Core.Clients.DTO;

namespace Rapido.Services.Customers.Core.Clients;

//TODO: Extract URL to the service registry
internal sealed class UserApiClient : IUserApiClient
{
    private const string ClientName = "consul";
    private const string ApiUrl = "http://users/users";
    private readonly IHttpClientFactory _clientFactory;
    
    public UserApiClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<UserDto> GetAsync(string email)
    {
        var client = _clientFactory.CreateClient(ClientName);
        return await client.GetFromJsonAsync<UserDto>($"{ApiUrl}/{email}");
    }
}