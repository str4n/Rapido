using Rapido.Framework.Auth;

namespace Rapido.Services.Users.Core.Storage;

public interface ITokenStorage
{
    void Set(JsonWebToken jwt);
    JsonWebToken Get();
}