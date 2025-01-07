using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Messaging.Brokers;

namespace Rapido.Framework.Testing.Abstractions;

public sealed class TestMessageBroker : IMessageBroker
{
    public static Queue<IMessage> Messages = new();

    public Task PublishAsync<T>(params T[] messages) where T : class, IMessage
    {
        foreach (var message in messages)
        {
            Messages.Enqueue(message);
        }

        return Task.CompletedTask;
    }
}