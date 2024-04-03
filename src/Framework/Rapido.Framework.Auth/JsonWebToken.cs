namespace Rapido.Framework.Auth;

public sealed record JsonWebToken(string Token, Guid UserId, string Email, string Role);