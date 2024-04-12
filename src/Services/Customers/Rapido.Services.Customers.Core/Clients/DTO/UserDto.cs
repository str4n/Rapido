namespace Rapido.Services.Customers.Core.Clients.DTO;

public sealed record UserDto(Guid UserId, string Email, string Role);