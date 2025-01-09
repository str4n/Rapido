using MassTransit;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Messages.Commands;

namespace Rapido.Services.Users.Core.UserActivation.Messages.Commands.Handlers;

internal sealed class CreateActivationTokenConsumer(IDispatcher dispatcher) : IConsumer<CreateActivationToken>
{
    public async Task Consume(ConsumeContext<CreateActivationToken> context)
    {
        var cancellationToken = context.CancellationToken;
        
        var message = context.Message;

        await dispatcher.DispatchAsync(message, cancellationToken);
    }
}