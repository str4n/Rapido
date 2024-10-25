namespace Rapido.Framework.Auth;

public sealed record JsonWebToken(string AccessToken, string RefreshToken, Guid UserId, string Email, string Role, DateTime Expires);