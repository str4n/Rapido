using Rapido.Services.Customers.Core.Clients;
using Rapido.Services.Customers.Core.Clients.DTO;

namespace Rapido.Tests.Services.Customers.Integration;

public class TestUserApiClient : IUserApiClient
{
    public Task<UserDto> GetAsync(string email)
        => Task.FromResult(new UserDto(Guid.NewGuid(), email, "user"));
}