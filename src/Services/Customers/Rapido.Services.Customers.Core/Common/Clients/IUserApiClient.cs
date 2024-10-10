using Rapido.Services.Customers.Core.Common.Clients.DTO;

namespace Rapido.Services.Customers.Core.Common.Clients;

internal interface IUserApiClient
{
    Task<UserDto> GetAsync(string email);
}