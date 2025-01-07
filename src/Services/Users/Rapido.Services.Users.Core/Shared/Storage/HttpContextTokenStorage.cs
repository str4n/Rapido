using Microsoft.AspNetCore.Http;
using Rapido.Framework.Auth;

namespace Rapido.Services.Users.Core.Shared.Storage;

internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Set(JsonWebToken jwt)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JsonWebToken Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }
        
        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JsonWebToken;
        }

        return null;
    }
}