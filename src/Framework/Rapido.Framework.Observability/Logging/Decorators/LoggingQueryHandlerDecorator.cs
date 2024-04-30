using Microsoft.Extensions.Logging;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Framework.Common.Attributes;
using Rapido.Framework.Contexts;

namespace Rapido.Framework.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;
    private readonly IContext _context;

    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler, ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
        IContext context)
    {
        _handler = handler;
        _logger = logger;
        _context = context;
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        var service = query.GetServiceName();
        var name = query.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.UserId;
        
        _logger.LogInformation("Handling a query: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, service, requestId, traceId, userId);

        var result = await _handler.HandleAsync(query);
        
        _logger.LogInformation("Handled a query: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, service, requestId, traceId, userId);

        return result;
    }
}