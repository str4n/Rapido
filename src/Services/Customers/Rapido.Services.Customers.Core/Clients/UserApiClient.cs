using System.Net.Http.Json;
using Rapido.Services.Customers.Core.Clients.DTO;

namespace Rapido.Services.Customers.Core.Clients;

//TODO: Extract URL to the service registry
internal sealed class UserApiClient : IUserApiClient
{
    private const string ApiUrl = "http://localhost:5020";
    private readonly IHttpClientFactory _clientFactory;
    
    public UserApiClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public Task<UserDto> GetAsync(string email)
    {
        var client = _clientFactory.CreateClient();
        return client.GetFromJsonAsync<UserDto>($"{ApiUrl}/users/{email}");
    }
}