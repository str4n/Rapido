using Rapido.Services.Customers.Core.Clients.DTO;

namespace Rapido.Services.Customers.Core.Clients;

internal interface IUserApiClient
{
    Task<UserDto> GetAsync(string email);
}