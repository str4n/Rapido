using Rapido.Services.Customers.Application.Common.Clients.DTO;

namespace Rapido.Services.Customers.Application.Common.Clients;

internal interface IUserApiClient
{
    Task<UserDto> GetAsync(string email);
}