using Rapido.Framework.Auth;

namespace Rapido.Services.Users.Core.User.DTO;

public sealed record AuthDto(JsonWebToken Token, string AccountType);