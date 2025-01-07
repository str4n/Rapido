namespace Rapido.Services.Users.Core.User.DTO;

public sealed record UserDto(Guid UserId, string Email, string Role, string AccountType);