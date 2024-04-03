using Microsoft.AspNetCore.Http;

namespace Rapido.Framework.Contexts.Factory;

internal sealed class ContextFactory : IContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public IContext Create()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        return httpContext is not null ? new Context(httpContext) : Context.Empty;
    }
}