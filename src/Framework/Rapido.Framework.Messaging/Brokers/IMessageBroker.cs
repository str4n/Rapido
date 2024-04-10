using Rapido.Framework.Common.Abstractions;

namespace Rapido.Framework.Messaging.Brokers;

public interface IMessageBroker
{
    Task PublishAsync<T>(params T[] messages) where T : class, IMessage;
}