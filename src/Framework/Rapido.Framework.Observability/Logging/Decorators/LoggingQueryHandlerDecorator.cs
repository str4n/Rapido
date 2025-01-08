using Microsoft.Extensions.Logging;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Framework.Common.Attributes;
using Rapido.Framework.Contexts;

namespace Rapido.Framework.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> handler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
    IContext context)
    : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var service = query.GetServiceName();
        var name = query.GetType().Name;
        var requestId = context.RequestId;
        var traceId = context.TraceId;
        var userId = context.Identity.UserId;
        
        logger.LogInformation("Handling a query: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, service, requestId, traceId, userId);

        var result = await handler.HandleAsync(query, cancellationToken);
        
        logger.LogInformation("Handled a query: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, service, requestId, traceId, userId);

        return result;
    }
}