namespace Rapido.Services.Customers.Core.Common.Clients.DTO;

public sealed record UserDto(Guid UserId, string Email, string Role);