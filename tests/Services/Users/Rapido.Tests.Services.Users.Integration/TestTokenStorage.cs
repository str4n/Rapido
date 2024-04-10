using Rapido.Framework.Auth;
using Rapido.Services.Users.Core.Storage;

namespace Rapido.Tests.Services.Users.Integration;

public class TestTokenStorage : ITokenStorage
{
    private const string Key = "jwt";
    private static Dictionary<string, JsonWebToken> _storage = new();

    public void Set(JsonWebToken jwt)
        => _storage.Add(Key, jwt);
    public JsonWebToken Get()
        => !_storage.TryGetValue(Key, out var jwt) ? null : jwt;
}