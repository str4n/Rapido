using Microsoft.AspNetCore.Http;
using Rapido.Framework.Contexts.Identity;

namespace Rapido.Framework.Contexts;

internal sealed class Context : IContext
{
    public Guid RequestId { get; } = Guid.NewGuid();
    public string TraceId { get; }
    public IIdentityContext Identity { get; }
    
    public Context() : this($"{Guid.NewGuid():N}")
    {
    }
    
    public Context(HttpContext context) : this(context.TraceIdentifier,
        new IdentityContext(context.User))
    {
    }
    
    public Context(string traceId, IIdentityContext identity = null)
    {
        TraceId = traceId;
        Identity = identity ?? IdentityContext.Empty;
    }
    
    public static IContext Empty => new Context();
}