using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions;
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
            busConfig.SetKebabCaseEndpointNameFormatter();
            
            var options = configuration.BindOptions<RabbitMqOptions>(SectionName);

            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName != null && x.FullName.Contains("Rapido.Services"))
                .ToArray();

            var types = assemblies
                .SelectMany(x => x.GetTypes()).ToArray();
            
            var consumerTypes = types
                .Where(x => x.IsClass && x.GetInterfaces().Any(t => t == typeof(IConsumer)))
                .ToArray();
            
            busConfig.AddConsumers(consumerTypes);
            
            busConfig.UsingRabbitMq((ctx, config) =>
            {
                config.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });

                config.Durable = options.Durable;
                
                config.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IMessageBroker, RabbitMqMessageBroker>();

        return services;
    }
}