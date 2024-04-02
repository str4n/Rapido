namespace Rapido.Framework.Auth.Authenticator;

public interface IAuthenticator
{
    JsonWebToken CreateToken(Guid userId, string role, string email);
}