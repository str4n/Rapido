namespace Rapido.Services.Users.Core.DTO;

public sealed record UserDto(Guid UserId, string Email, string Role, DateTime CreatedAt);