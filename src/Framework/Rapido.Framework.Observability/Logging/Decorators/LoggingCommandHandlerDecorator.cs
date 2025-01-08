using Microsoft.Extensions.Logging;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Attributes;
using Rapido.Framework.Contexts;

namespace Rapido.Framework.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
    IContext context)
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var service = command.GetServiceName();
        var name = command.GetType().Name;
        var requestId = context.RequestId;
        var traceId = context.TraceId;
        var userId = context.Identity.UserId;
        
        logger.LogInformation("Handling a command: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, service, requestId, traceId, userId);

        await handler.HandleAsync(command, cancellationToken);
        
        logger.LogInformation("Handled a command: {Name} ({Service}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, service, requestId, traceId, userId);
    }
}