using Rapido.Framework.Common.Abstractions;

namespace Rapido.Framework.Messaging.Brokers;

public interface IMessageBroker
{
    Task PublishAsync<T>(T message) where T : class, IMessage;
}