using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Messaging.RabbitMQ.Brokers;

namespace Rapido.Framework.Messaging.RabbitMQ;

public static class Extensions
{
    private const string SectionName = "rabbitMQ";
    
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfig =>
        {
            var options = configuration.BindOptions<RabbitMqOptions>(SectionName);
            
            busConfig.UsingRabbitMq((ctx, config) =>
            {
                config.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });

                config.Durable = options.Durable;
            });
            
            busConfig.SetKebabCaseEndpointNameFormatter();
        });

        services.AddScoped<IMessageBroker, RabbitMqMessageBroker>();

        return services;
    }
}