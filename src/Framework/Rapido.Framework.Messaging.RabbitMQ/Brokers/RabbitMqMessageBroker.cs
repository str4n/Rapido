using MassTransit;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Messaging.Brokers;

namespace Rapido.Framework.Messaging.RabbitMQ.Brokers;

internal sealed class RabbitMqMessageBroker : IMessageBroker
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RabbitMqMessageBroker> _logger;

    public RabbitMqMessageBroker(IPublishEndpoint publishEndpoint, ILogger<RabbitMqMessageBroker> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }
    
    public Task PublishAsync<T>(params T[] messages) where T : class, IMessage
    {
        if (messages is null)
        {
            return Task.CompletedTask;
        }

        messages = messages.Where(x => x is not null).ToArray();

        foreach (var message in messages)
        {
            _logger.LogInformation("Publishing a message: {message}", message);
            _publishEndpoint.Publish(message);
        }

        return Task.CompletedTask;
    }
}