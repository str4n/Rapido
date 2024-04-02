using Rapido.Framework.Contexts.Identity;

namespace Rapido.Framework.Contexts;

public interface IContext
{
    Guid RequestId { get; }
    string TraceId { get; }
    IIdentityContext Identity { get; }
}