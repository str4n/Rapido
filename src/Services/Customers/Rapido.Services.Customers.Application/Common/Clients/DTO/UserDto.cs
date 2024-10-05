namespace Rapido.Services.Customers.Application.Common.Clients.DTO;

public sealed record UserDto(Guid UserId, string Email, string Role);