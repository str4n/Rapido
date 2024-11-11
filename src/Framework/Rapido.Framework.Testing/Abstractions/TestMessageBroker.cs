using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Messaging.Brokers;

namespace Rapido.Framework.Testing.Abstractions;

public sealed class TestMessageBroker : IMessageBroker
{
    public Task PublishAsync<T>(params T[] messages) where T : class, IMessage
        => Task.CompletedTask;
}