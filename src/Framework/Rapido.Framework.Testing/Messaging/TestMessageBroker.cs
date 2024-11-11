using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Messaging.Brokers;
using Testcontainers.RabbitMq;

namespace Rapido.Framework.Testing.Messaging;

public static class TestMessageBroker
{
    private const string Name = "rabbitmq-test";
    private const int Port = 5672;
    
    public static async Task<RabbitMqContainer> InitRabbitMqAsync()
    {
        var container = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.11.7-management")
            .WithExposedPort(Port)
            .WithPortBinding(Port, true)
            .WithName($"{Name}-{Guid.NewGuid():N}")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await container.StartAsync();
        return container;
    }
}