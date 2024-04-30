using Microsoft.Extensions.Logging;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Attributes;
using Rapido.Framework.Contexts;

namespace Rapido.Framework.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;
    private readonly IContext _context;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> handler, ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
        IContext context)
    {
        _handler = handler;
        _logger = logger;
        _context = context;
    }
    
    public async Task HandleAsync(TCommand command)
    {
        var service = command.GetServiceName();
        var name = command.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.UserId;
        
        _logger.LogInformation("Handling a command: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, service, requestId, traceId, userId);

        await _handler.HandleAsync(command);
        
        _logger.LogInformation("Handled a command: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, service, requestId, traceId, userId);
    }
}