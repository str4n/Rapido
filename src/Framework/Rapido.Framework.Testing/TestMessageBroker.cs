using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Messaging.Brokers;

namespace Rapido.Framework.Testing;

public class TestMessageBroker : IMessageBroker
{
    
    //TODO: test message broker
    public Task PublishAsync<T>(params T[] messages) where T : class, IMessage
        => Task.CompletedTask;
}